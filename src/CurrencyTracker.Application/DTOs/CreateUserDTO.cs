namespace CurrencyTracker.Application.DTOs;

public class CreateUserDTO
{
  public string FullName {get; set;} = default!;
  public string Email {get ;set;} = default!;
  public string Password {get; set;} = default!;
}
