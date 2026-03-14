using System.ComponentModel.DataAnnotations;

namespace CurrencyTracker.Application.DTOs.Users;

public class ForgotPasswordDTO
{         
     [Required]
     [EmailAddress]
          public string Email {get;set;} = default!;
     
}
