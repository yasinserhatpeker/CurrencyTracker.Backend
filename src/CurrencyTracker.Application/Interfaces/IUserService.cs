using System;
using System.Data;
using CurrencyTracker.Application.DTOs;

namespace CurrencyTracker.Application.Interfaces;

public interface IUserService
{
   Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
   Task CreateUserAsync(CreateUserDTO createUserDTO);
   Task<UserResponseDTO> GetByIdAsync();
   Task UpdateUserAsync(Guid id, CreateUserDTO createUserDTO);
   Task DeleteUserAsync(Guid id);
   




}
