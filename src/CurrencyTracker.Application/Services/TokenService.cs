using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.Helpers;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;
using CurrencyTracker.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyTracker.Application.Services;

public class TokenService : ITokenService
{   
    private readonly IConfiguration _configuration;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;

    public TokenService(IConfiguration configuration, IGenericRepository<User> userRepository,IGenericRepository<RefreshToken> refreshTokenRepository)
    {
        _configuration=configuration;
        _userRepository=userRepository;
        _refreshTokenRepository=refreshTokenRepository;
    }
    public async Task<AuthResponseDTO> GenerateAuthResponseAsync(User user)
    {
         var accesToken = GenerateAccessToken(user);
         var refreshToken = CryptoHelpers.GenerateSecureToken();

         var newSession = new RefreshToken
         {
            Id=Guid.NewGuid(),
            HashToken=CryptoHelpers.HashToken(refreshToken),
            UserId = user.Id,
            ExpiryTime = DateTime.UtcNow.AddDays(7) 
         };

         await _refreshTokenRepository.AddAsync(newSession);
         return new AuthResponseDTO
         {
             AccessToken=accesToken,
             RefreshToken=refreshToken
         };

    }

    public async Task LogoutAsync(RefreshTokenDTO refreshTokenDTO)
    {
       var hashed = CryptoHelpers.HashToken(refreshTokenDTO.RefreshToken);

       var refreshTokens = await _refreshTokenRepository.Find(u=> u.HashToken == hashed);
       var refreshToken = refreshTokens.FirstOrDefault();
       
       if(refreshToken is not null)
        {
            await _refreshTokenRepository.DeleteAsync(refreshToken.Id);
        }

    }

    public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO)
    {
        var hashed = CryptoHelpers.HashToken(refreshTokenDTO.RefreshToken);
        var refreshTokens = await _refreshTokenRepository.Find(u=>u.HashToken == hashed);
        var refreshToken = refreshTokens.FirstOrDefault();

        if(refreshToken is null || refreshToken.ExpiryTime < DateTime.UtcNow || refreshToken.ExpiryTime is null)
        {
            throw new KeyNotFoundException("Session is expired please try again.");
        }
        var user = await _userRepository.GetByIdAsync(refreshToken.UserId);
        if(user is null)
        {
             throw new KeyNotFoundException("Cannot retrieve user data");
        }

          await _refreshTokenRepository.DeleteAsync(refreshToken.Id);

          return await GenerateAuthResponseAsync(user);
    }

   

    private string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()) // token-refreshing with token ID

         };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

        var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

       var token = new JwtSecurityToken
        (
           issuer:_configuration["JwtSettings:Issuer"],
           audience:_configuration["JwtSettings:Audience"],
           claims: claims,
           expires:DateTime.UtcNow.AddMinutes(10),
           signingCredentials:creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
