using CurrencyTracker.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.ServiceRegistration;

public  static class ApiRegistration
{
  public static void AddApiRegistrationServices(this IServiceCollection services)
    {
        services.AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
          options.InvalidModelStateResponseFactory = context =>
          {
              var errors = context.ModelState
              .Where(e => e.Value?.Errors.Count > 0)
              .SelectMany(x => x.Value.Errors)
              .Select(x=>x.ErrorMessage)
              .ToList();


              var responseObj = ApiResponse<object>.Fail(errors,"Validation errors occured");
              return new BadRequestObjectResult(responseObj);
          };

        });
        services.AddEndpointsApiExplorer();
    }

   
}
