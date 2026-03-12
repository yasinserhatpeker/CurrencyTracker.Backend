namespace CurrencyTracker.Application.Models;

public class ExternalUserInfo
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ProviderUserId { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
}
