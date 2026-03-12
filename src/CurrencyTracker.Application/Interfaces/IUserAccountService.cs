using CurrencyTracker.Application.DTOs.Users;

namespace CurrencyTracker.Application.Interfaces;

public interface IUserAccountService
{
    Task<bool> EmailVerificationAsync(string token);
    Task ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO);
    Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
}
