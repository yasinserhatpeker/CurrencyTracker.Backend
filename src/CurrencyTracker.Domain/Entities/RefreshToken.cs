namespace CurrencyTracker.Domain.Entities;

public class RefreshToken : BaseEntity
{
     public string HashToken {get;set;} = string.Empty;
     public DateTime? ExpiryTime {get;set;}

     // foreign key mapping to the user
     public User User {get;set;} = null!;
     public Guid UserId{get;set;} 
}
