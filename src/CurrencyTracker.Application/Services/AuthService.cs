using System;
using AutoMapper;
using BCrypt.Net;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;
using CurrencyTracker.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

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
       var users = await _userRepository.GetAllAsync(); // for mvp it'll change later
       if(users.Any(u =>u.Email == createUserDTO.Email) )
        {
            throw new Exception("This email is already used");
        }
        var user = _mapper.Map<User>(createUserDTO);

        //password hashing
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDTO.Password);
        user.AuthProvider="Local";

        await _userRepository.AddAsync(user);
        return _mapper.Map<UserResponseDTO>(user);

    }
    public async Task<AuthResponseDTO> LoginAsync(LoginUserDTO loginUserDTO)
    {
        var users = await _userRepository.GetAllAsync(); // for mvp it'll change later
        var user = users.FirstOrDefault(u=> u.Email == loginUserDTO.Email);

        if(user is null || user.PasswordHash is null || !BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.PasswordHash))
        {
           throw new Exception("Invalid email or password.");
        }
        return await GenerateAuthResponseAsync(user); // helper class for less code
    }

    public async Task<AuthResponseDTO> GenerateAuthResponseAsync(User user)
    {
        var accesToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime=DateTime.UtcNow.AddDays(7); // 7-days refresh token

         await _userRepository.UpdateAsync(user);

         return new AuthResponseDTO
         {
             AccessToken=accesToken,
             RefreshToken=refreshToken
         };

    }



    public Task<AuthResponseDTO> RefreshTokenAsync(string RefreshToken)
    {
        throw new NotImplementedException();
    }

}
