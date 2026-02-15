using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.DTOs.Users;

namespace CurrencyTracker.Application.Interfaces;

public interface IAuthService
{
    Task<UserResponseDTO> RegisterAsync(CreateUserDTO createUserDTO); // registering user
    Task<AuthResponseDTO> LoginAsync(LoginUserDTO loginUserDTO);   // logging user
    Task<AuthResponseDTO> RefreshTokenAsync(string RefreshToken); // new refresh token generate
     Task<GoogleLoginDTO> GoogleLoginAsync (GoogleLoginDTO googleLoginDTO); // OAuth2.0 Google entegration 

}
