using System;
using CurrencyTracker.Domain.Entities.Enums;

namespace CurrencyTracker.Application.DTOs.Transactions;

public class CreateTransactionsDTO
{
    public Guid PortfolioId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Symbol { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public decimal Quantity { get; set; } = default!;
    public TransactionType TransactionType {get;set;}
}
