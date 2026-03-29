namespace CurrencyTracker.Domain.Entities;

public class RefreshToken : BaseEntity
{
     public string HashToken {get;set;} = string.Empty;
     public DateTime? ExpiryTime {get;set;}
     public User User {get;set;} = null!; // navigation property
     public Guid UserId{get;set;} // foreign key
}
