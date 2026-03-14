using System.ComponentModel.DataAnnotations;

namespace CurrencyTracker.Application.DTOs.Auth;

public class RefreshTokenDTO
{  
       [Required(ErrorMessage = "Refresh Token is required")]    
        public string RefreshToken {get;set;} = string.Empty;
}
