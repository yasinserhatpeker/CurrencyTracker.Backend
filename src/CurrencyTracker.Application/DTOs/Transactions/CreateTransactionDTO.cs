using System;

namespace CurrencyTracker.Application.DTOs.Transactions;

public class CreateTransactionsDTO
{
    public Guid PortfolioId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Symbol { get; set; } = default!;
    public string Price { get; set; } = default!;
    public string Quantity { get; set; } = default!;
}
