using System;
using System.Data;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Users;

namespace CurrencyTracker.Application.Interfaces;

public interface IUserService
{
   Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
   Task CreateUserAsync(CreateUserDTO createUserDTO);
   Task<UserResponseDTO> GetByIdAsync(Guid id);
   Task UpdateUserAsync(Guid id, UpdateUserDTO updateUserDTO);
   Task RemoveUserAsync(Guid id);
   
}
