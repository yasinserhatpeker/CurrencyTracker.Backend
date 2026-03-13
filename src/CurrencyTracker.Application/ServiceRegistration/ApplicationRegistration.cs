using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyTracker.Application.ServiceRegistration;

   public static class ApplicationRegistration
{
      public static void AddApplicationServices(this IServiceCollection services)
    {
       services.AddScoped<IUserService, UserService>();
       services.AddAutoMapper(typeof(ApplicationRegistration));
       services.AddScoped<IPortfolioService, PortfolioService>();
       services.AddScoped<ITransactionService, TransactionService>();
       services.AddScoped<IAuthService, AuthService>();
       services.AddScoped<IUserAccountService,UserAccountService>();
      
    }
}


