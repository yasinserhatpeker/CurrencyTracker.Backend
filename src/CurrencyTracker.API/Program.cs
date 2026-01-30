using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Services;
using CurrencyTracker.Domain.Interfaces;
using CurrencyTracker.Infrastructure.Implementations;
using CurrencyTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IPortfolioService,PortfolioService>();
builder.Services.AddScoped<ITransactionService,TransactionService>();
builder.Services.AddScoped<IAuthService,AuthService>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
  
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
