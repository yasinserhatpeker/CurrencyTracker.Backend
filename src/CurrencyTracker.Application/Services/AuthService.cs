using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;
using CurrencyTracker.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyTracker.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IMapper mapper, IGenericRepository<User> userRepository, IConfiguration configuration)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _configuration = configuration;
    }
    public async Task<UserResponseDTO> RegisterAsync(CreateUserDTO createUserDTO)
    {
        var existingUsers = await _userRepository.Find(u => u.Email == createUserDTO.Email);
        if (existingUsers.Any())
        {
            throw new Exception("This email is already used");
        }
        var user = _mapper.Map<User>(createUserDTO);

        //password hashing
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDTO.Password);
        user.AuthProvider = "Local";

        await _userRepository.AddAsync(user);
        return _mapper.Map<UserResponseDTO>(user);

    }
    public async Task<AuthResponseDTO> LoginAsync(LoginUserDTO loginUserDTO)
    {
        var users = await _userRepository.Find(u => u.Email == loginUserDTO.Email);
        var user = users.FirstOrDefault();

        if (user is null || user.PasswordHash is null || !BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.PasswordHash))
        {
            throw new Exception("Invalid email or password.");
        }
        return await GenerateAuthResponseAsync(user); // helper method for less code
    }

    public async Task<AuthResponseDTO> RefreshTokenAsync(string RefreshToken)
    {
        var users = await _userRepository.Find(u => u.RefreshToken == RefreshToken);
        var user = users.FirstOrDefault();

        if (user is null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            throw new Exception("The session is expired. Please try again");
        }
        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponseDTO> GenerateAuthResponseAsync(User user)
    {
        var accesToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // 7-days refresh token

        await _userRepository.UpdateAsync(user);

        return new AuthResponseDTO
        {
            AccessToken = accesToken,
            RefreshToken = refreshToken
        };

    }
    private string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
          new Claim(JwtRegisteredClaimNames.Sub ,user.Id.ToString()),
          new Claim(JwtRegisteredClaimNames.Email, user.Email),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
  );
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);

        }
    }

}
