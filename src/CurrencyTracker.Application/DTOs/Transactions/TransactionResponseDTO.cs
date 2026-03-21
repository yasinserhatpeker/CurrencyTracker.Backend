using CurrencyTracker.Domain.Entities.Enums;

namespace CurrencyTracker.Application.DTOs.Transactions;

public class TransactionResponseDTO
{
   public Guid Id {get;set;}
   public Guid PortfolioId {get;set;} 
   public decimal Price {get;set;}
   public decimal Quantity {get;set;}
   public string QuoteCurrency { get; set; } = default!;
   public DateTime TransactionDate {get;set;} 
   public string BaseCurrency {get;set;} = default!;
   public decimal TotalValue {get;set;}
   public TransactionType TransactionType {get;set;}

}
