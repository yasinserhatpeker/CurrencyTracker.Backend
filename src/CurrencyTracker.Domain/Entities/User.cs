using System;

namespace CurrencyTracker.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;  
    public string AuthProvider { get; set; } = default!;
    public string GoogleId {get;set;} = default!;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string Email { get; set; } = string.Empty;
    public ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();

}

