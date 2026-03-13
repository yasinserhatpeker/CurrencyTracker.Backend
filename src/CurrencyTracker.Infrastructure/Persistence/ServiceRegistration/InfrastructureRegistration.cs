using System.Text;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Services;
using CurrencyTracker.Infrastructure.Authentication;
using CurrencyTracker.Infrastructure.EmailVerification;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyTracker.Infrastructure.Persistence.ServiceRegistration;

public static class InfrastructureRegistration
{
   
    public static void AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {
         services.AddScoped<ITokenService,JwtTokenService>();
         services.AddScoped<IExternalAuthProvider, GoogleAuthProvider>();
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

      
         
        
    }
}
