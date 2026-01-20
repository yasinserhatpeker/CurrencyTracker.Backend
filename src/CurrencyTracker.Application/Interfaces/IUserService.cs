using System;
using CurrencyTracker.Application.DTOs;

namespace CurrencyTracker.Application.Interfaces;

public interface IUserService
{
   Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
   Task  CreateUserAsync(CreateUserDTO createUserDTO);
}
