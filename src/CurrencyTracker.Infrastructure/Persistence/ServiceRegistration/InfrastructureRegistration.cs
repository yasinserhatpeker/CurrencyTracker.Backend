using System.Text;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Services;
using CurrencyTracker.Infrastructure.Authentication;
using CurrencyTracker.Infrastructure.EmailVerification;
using CurrencyTracker.Infrastructure.ExternalApis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;

namespace CurrencyTracker.Infrastructure.Persistence.ServiceRegistration;

public static class InfrastructureRegistration
{
   
    public static void AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {
         services.AddScoped<ITokenService,JwtTokenService>();
         services.AddScoped<IExternalAuthProvider,GoogleAuthProvider>();
         services.AddScoped<IEmailService, MockEmailService>();

         var jwtSettings = configuration.GetSection("JwtSettings");
         var secretKey = jwtSettings["SecretKey"];

         services.AddAuthentication(options =>
         {
             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         })
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 ValidIssuer = jwtSettings["Issuer"],
                 ValidAudience = jwtSettings["Audience"],
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
                 ClockSkew = TimeSpan.Zero
             };
         });
         
         services.AddTransient<LoggingHandler>();

         services.AddHttpClient<IPriceProvider, FrankfurterClient>(client =>
         {
             client.BaseAddress =new Uri(configuration["ExternalApis:FrankfurterApi"] ?? throw new InvalidCastException("FrankfurterApi is not found in the configuration file"));
             client.Timeout = TimeSpan.FromSeconds(10);
         })
         .AddHttpMessageHandler<LoggingHandler>()
         .AddPolicyHandler(GetResiliencePolicy());

    }

    public static IAsyncPolicy<HttpResponseMessage> GetResiliencePolicy()
    {
        var retryPolicy = HttpPolicyExtensions // retry mechanism that retries 3 times
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var circuitBreaker = HttpPolicyExtensions // a circuit breaker, after 5 consecutive failures, close the fkin door for 30 seconds
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        return Policy.WrapAsync(retryPolicy, circuitBreaker);
    }
}

    

