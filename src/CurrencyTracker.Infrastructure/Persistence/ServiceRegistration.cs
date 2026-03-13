using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Services;
using CurrencyTracker.Domain.Interfaces;
using CurrencyTracker.Infrastructure.Authentication;
using CurrencyTracker.Infrastructure.EmailVerification;
using CurrencyTracker.Infrastructure.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyTracker.Infrastructure.Persistence;

public static class ServiceRegistration
{
   public static void AddPersistenceServices(this IServiceCollection services)
    {
       services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
       services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
       services.AddScoped<IUserService, UserService>();
       services.AddScoped<IPortfolioService, PortfolioService>();
       services.AddScoped<ITransactionService, TransactionService>();
       services.AddScoped<IAuthService, AuthService>();
       services.AddScoped<ITokenService,JwtTokenService>();
       services.AddScoped<IUserAccountService,UserAccountService>();
       services.AddScoped<IExternalAuthProvider, GoogleAuthProvider>();
       services.AddScoped<IEmailService, MockEmailService>();

    }
}
