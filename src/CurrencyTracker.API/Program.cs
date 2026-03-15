using CurrencyTracker.API.Middlewares;
using CurrencyTracker.API.ServiceRegistration;
using CurrencyTracker.Application.ServiceRegistration;
using CurrencyTracker.Infrastructure.Persistence;
using CurrencyTracker.Infrastructure.Persistence.ServiceRegistration;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSwaggerGenServices();
builder.Services.AddApiRegistrationServices();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
