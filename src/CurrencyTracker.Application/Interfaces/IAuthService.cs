using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.DTOs.Users;

namespace CurrencyTracker.Application.Interfaces;

public interface IAuthService
{
    Task<UserResponseDTO> RegisterAsync(CreateUserDTO createUserDTO); // registering user
    Task<AuthResponseDTO> LoginAsync(LoginUserDTO loginUserDTO);   // logging user
    Task<AuthResponseDTO> GoogleLoginAsync(GoogleLoginDTO googleLoginDTO); // google oauth2.0 implementation
}
