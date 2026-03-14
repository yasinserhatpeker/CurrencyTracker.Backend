using System;
using System.Data;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Users;

namespace CurrencyTracker.Application.Interfaces;

public interface IUserService
{
   Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
   Task<UserResponseDTO> CreateUserAsync(CreateUserDTO createUserDTO);
   Task<UserResponseDTO> GetByIdAsync(Guid id);
   Task<UserResponseDTO> UpdateUserAsync(Guid id, UpdateUserDTO updateUserDTO);
   Task DeleteUserAsync(Guid id);
   
}
