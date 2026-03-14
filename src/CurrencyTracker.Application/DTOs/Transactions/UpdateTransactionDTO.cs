using System.ComponentModel.DataAnnotations;
using CurrencyTracker.Domain.Entities.Enums;

namespace CurrencyTracker.Application.DTOs.Transactions;

public class UpdateTransactionDTO
{
  public Guid Id{get;set;}

  public Guid PortfolioId {get;set;}
  [Required]
  public TransactionType TransactionType {get;set;}
  [Required]
  [StringLength(10,MinimumLength =2)]
  public string Symbol {get;set;} = default!;
  [Range(0.00000001,double.MaxValue)]
  public decimal Price {get;set;} 
  [Range(0.00000001,double.MaxValue)]
  public decimal Quantity{get;set;}
  [Required]
  public DateTime TransactionDate{get;set;}

}
