namespace CurrencyTracker.Application.DTOs;

public class UserResponseDTO
{
  public Guid Id {get; set;}
  public string Username {get;set;} = default!;
  public string Email {get ; set;} = default!;

}
