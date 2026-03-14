namespace CurrencyTracker.API.ServiceRegistration;

public  static class ApiRegistration
{
  public static void AddApiRegistrationServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
    }

   
}
