using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyTracker.Application.DTOs.Users;

public class LoginUserDTO
{
  [Required]
  public string Email {get;set;} = default!;
  [Required]
  public string Password {get;set;} = default!;
}
