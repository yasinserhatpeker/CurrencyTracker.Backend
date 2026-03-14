using CurrencyTracker.Application.Interfaces;
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
