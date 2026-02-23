using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.DTOs.Users;

namespace CurrencyTracker.Application.Interfaces;

public interface IAuthService
{
    Task<UserResponseDTO> RegisterAsync(CreateUserDTO createUserDTO); // registering user
    Task<AuthResponseDTO> LoginAsync(LoginUserDTO loginUserDTO);   // logging user
    Task LogoutAsync(Guid userId); // logging out user
    Task<AuthResponseDTO> RefreshTokenAsync(string RefreshToken); // new refresh token generate
    Task<AuthResponseDTO> GoogleLoginAsync (GoogleLoginDTO googleLoginDTO); // OAuth2.0 Google entegration 
    Task ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO); // forgot my password
    Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);   // reset password
    Task EmailVerificationAsync(string token);  // email verification




}
