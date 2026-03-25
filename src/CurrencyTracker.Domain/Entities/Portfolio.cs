namespace CurrencyTracker.Domain.Entities;

public class Portfolio : BaseEntity
{
  public string Name { get; set; } = string.Empty;
  public Guid UserId { get; set; }
  public User User { get; set; } = default!;
  public string DisplayCurrency { get; set; } = string.Empty;
  public string 
  public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();


}
