using System;
using AutoMapper;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;
using CurrencyTracker.Domain.Interfaces;

namespace CurrencyTracker.Application.Services;

public class AuthService : IAuthService
{
     private readonly IMapper _mapper;
     private readonly IGenericRepository<User> _userRepository;
     
     private readonly IConfiguration _configuration;
    public Task<AuthResponseDTO> LoginUserAsync(LoginUserDTO loginUserDTO)
    {
        throw new NotImplementedException();
    }

    public Task<AuthResponseDTO> RefreshTokenAsync(string RefreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponseDTO> RegisterUserAsync(CreateUserDTO createUserDTO)
    {
        throw new NotImplementedException();
    }
}
