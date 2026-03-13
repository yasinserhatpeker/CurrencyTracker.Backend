using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Services;
using CurrencyTracker.Domain.Interfaces;
using CurrencyTracker.Infrastructure.Authentication;
using CurrencyTracker.Infrastructure.EmailVerification;
using CurrencyTracker.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CurrencyTracker.Infrastructure.Persistence;

public static class PersistenceRegistration
{
   public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
       services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
       services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

      
    }

  
}
