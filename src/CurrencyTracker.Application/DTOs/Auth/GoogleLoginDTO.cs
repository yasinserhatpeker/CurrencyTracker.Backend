using System.ComponentModel.DataAnnotations;

namespace CurrencyTracker.Application.DTOs.Auth;

public class GoogleLoginDTO
{      
   [Required(ErrorMessage = "IdToken is required")]  
   public string IdToken {get;set;} = default!;
}
