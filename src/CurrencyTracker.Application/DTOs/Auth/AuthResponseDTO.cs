namespace CurrencyTracker.Application.DTOs.Auth;

public class AuthResponseDTO
{
   public string AccessToken {get;set;} = default!;
   public string RefreshToken {get;set;} = default!;
}
