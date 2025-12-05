using System;
using CurrencyTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTracker.Infrastructure.Persistence;

public class AppDbContext:DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users {get; set;}
    public DbSet<Portfolio> Portfolios {get; set;}
    public DbSet<Transaction> Transactions {get; set;}
    
}
