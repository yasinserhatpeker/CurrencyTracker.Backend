using System.Reflection;
using CurrencyTracker.Domain.Entities;
using CurrencyTracker.Infrastructure.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTracker.Infrastructure.Persistence;

public class AppDbContext:DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public DbSet<User> Users {get; set;}
    public DbSet<Portfolio> Portfolios {get; set;}
    public DbSet<Transaction> Transactions {get; set;}
    
}
