using System.ComponentModel.DataAnnotations;

namespace CurrencyTracker.Application.DTOs.Users;

public class ResetPasswordDTO
{ 
  [Required]
  [MinLength(6)]
  public string NewPassword{get;set;} = default!;
  [Compare("NewPassword",ErrorMessage = "Passwords do not match")]
  public string ConfirmPassword{get;set;} = default!;
  [Required]
  public string ResetPasswordToken {get;set;} = default!;

}
