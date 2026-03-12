using System;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Domain.Entities;

namespace CurrencyTracker.Application.Interfaces;

public interface ITokenService
{
     Task<AuthResponseDTO> GenerateAuthResponseAsync(User user); // Generating response
     Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO); // new refresh token generate
     Task LogoutAsync(RefreshTokenDTO refreshTokenDTO); // logging out user
    
}
