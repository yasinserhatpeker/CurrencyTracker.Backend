namespace CurrencyTracker.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string? ResetPasswordTokenHash {get;set;}
    public DateTime? ResetPasswordTokenExpiryTime{get;set;}

    public string AuthProvider { get; set; } = "Local";
    public string? GoogleId {get;set;}

    public string? RefreshTokenHash {get;set;}
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public bool IsEmailVerified {get;set;} = false;
    public string? EmailVerificationTokenHash {get;set;}
    
    public string Email { get; set; } = string.Empty;
    public ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>(); // one to many
    public ICollection<RefreshToken> RefreshTokens {get;set;} = new List<RefreshToken>(); // one to many

}

