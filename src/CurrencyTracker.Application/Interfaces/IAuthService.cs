using System;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.DTOs.Users;

namespace CurrencyTracker.Application.Interfaces;

public interface IAuthService
{
    Task<UserResponseDTO> RegisterUserAsync(CreateUserDTO createUserDTO); // registering user
    Task<AuthResponseDTO> LoginUserAsync(LoginUserDTO loginUserDTO);   // logging user
    Task<AuthResponseDTO> RefreshTokenAsync(string RefreshToken); // new refresh token generate

}
