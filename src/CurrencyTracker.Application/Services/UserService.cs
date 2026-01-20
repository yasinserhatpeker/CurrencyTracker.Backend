using System;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;

namespace CurrencyTracker.Application.Services;

public class UserService : IUserService
{
     
    public Task CreateUserAsync(CreateUserDTO createUserDTO)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }
}
