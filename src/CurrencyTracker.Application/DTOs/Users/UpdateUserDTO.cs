using System;

namespace CurrencyTracker.Application.DTOs.Users;

public class UpdateUserDTO
{
   public string Email {get;set;} = default!;
   public string Password {get; set;} = default!;
   public string Username {get;set;} = default!;

}
