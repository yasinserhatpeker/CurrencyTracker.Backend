using System.ComponentModel.DataAnnotations;
using CurrencyTracker.Domain.Entities.Enums;

namespace CurrencyTracker.Application.DTOs.Transactions;

public class CreateTransactionsDTO
{

    
    public Guid PortfolioId { get; set; }
    
    public DateTime TransactionDate { get; set; }

   
    public string BaseCurrency { get; set; } = default!;

    
    public decimal Price { get; set; } = default!;

   
    public decimal Quantity { get; set; } = default!;

   
    public TransactionType TransactionType { get; set; }

    public string QuoteCurrency { get; set; } = "TRY";
}
