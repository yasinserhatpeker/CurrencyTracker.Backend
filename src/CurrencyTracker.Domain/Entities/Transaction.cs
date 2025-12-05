using System;
using CurrencyTracker.Domain.Entities.Enums;

namespace CurrencyTracker.Domain.Entities;

public class Transaction : BaseEntity
{ 
    public Guid PortfolioId {get;set;} 
    public Portfolio Portfolio {get;set;} = default!;
    public DateTime TransactionDate {get;set;}
    public string Symbol {get;set;} = string.Empty;
    public decimal Price {get;set;}
    public decimal Quantity{get;set;}  
    public TransactionType TransactionType {get;set;}
    

}
