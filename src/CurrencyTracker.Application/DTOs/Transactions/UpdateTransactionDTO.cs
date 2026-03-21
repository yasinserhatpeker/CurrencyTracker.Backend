using System.ComponentModel.DataAnnotations;
using CurrencyTracker.Domain.Entities.Enums;

namespace CurrencyTracker.Application.DTOs.Transactions;

public class UpdateTransactionDTO
{
  public Guid Id{get;set;}
  public Guid PortfolioId {get;set;}
  public TransactionType TransactionType {get;set;}
  public decimal Quantity{get;set;}
  public decimal Price{get;set;}
  public DateTime TransactionDate{get;set;}
  
}
