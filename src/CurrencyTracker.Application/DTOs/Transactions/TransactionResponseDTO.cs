using CurrencyTracker.Domain.Entities.Enums;

namespace CurrencyTracker.Application.DTOs.Transactions;

public class TransactionResponseDTO
{
   public DateTime TransactionDate {get;set;} 
   public string Symbol {get;set;} = default!;
   public decimal TotalValue {get;set;}
   public TransactionType TransactionType {get;set;}

}
