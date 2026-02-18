namespace CurrencyTracker.Application.DTOs.Users;

public class ResetPasswordDTO
{
  public string NewPassword{get;set;} = default!;
  public string ConfirmPassword{get;set;} = default!;
  public string ResetPasswordToken {get;set;} = default!;

}
