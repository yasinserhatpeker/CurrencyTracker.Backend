using System;

namespace CurrencyTracker.Application.DTOs.Users;

public class LoginUserDTO
{
  public string Email {get;set;} = default!;
  public string Password {get;set;} = default!;
}
