using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CurrencyTracker.Domain.Entities;

public abstract class BaseEntity
{
  public Guid Id {get; set;}
  public DateTime CreatedAt {get; set;}
  public DateTime UpdatedAt {get; set;} 
  public bool IsDeleted {get; set;}
  public DateTime? DeleteTime {get; set;}
  public Guid? CreatedBy {get; set;}
  public Guid? UpdatedBy{get; set;}

}
