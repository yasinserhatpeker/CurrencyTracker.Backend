using System;
using AutoMapper;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;

namespace CurrencyTracker.Application.Services;

public class UserService : IUserService
{  
    private readonly Mapper _mapper;
    public UserService(Mapper mapper)
    {
        _mapper =mapper;
    }
    

     
    public Task CreateUserAsync(CreateUserDTO createUserDTO)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }
}
