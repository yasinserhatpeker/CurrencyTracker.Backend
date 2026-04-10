using Microsoft.EntityFrameworkCore;
using CurrencyTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyTracker.Infrastructure.Persistence.EntityConfigurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(t=>t.Price).HasPrecision(18,8);
        builder.Property(t=>t.Quantity).HasPrecision(18,8);
    }
}

