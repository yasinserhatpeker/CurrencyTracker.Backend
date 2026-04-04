using CurrencyTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTracker.Infrastructure.Persistence;

public class AppDbContext:DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
        
        modelBuilder.Entity<Transaction>(builder =>
        {
            builder.Property(t=>t.Price).HasPrecision(18,8);
            builder.Property(t=>t.Quantity).HasPrecision(18,8);         
            });
    }
    public DbSet<User> Users {get; set;}
    public DbSet<Portfolio> Portfolios {get; set;}
    public DbSet<Transaction> Transactions {get; set;}
    
}
