using System;
using AutoMapper;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;
using CurrencyTracker.Domain.Interfaces;

namespace CurrencyTracker.Application.Services;

public class UserService : IUserService
{  
    private readonly IMapper _mapper;
    private readonly IGenericRepository<User> _userRepository;

    public UserService(IMapper mapper, IGenericRepository<User> userRepository)
    {
        _mapper=mapper;
        _userRepository=userRepository;
    }


     
    public async Task CreateUserAsync(CreateUserDTO createUserDTO)
    {
        var newUser = _mapper.Map<User>(createUserDTO);
        await _userRepository.AddAsync(newUser);

    }

    public Task DeleteUserAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        var users = _userRepository.GetAll();
        var mappedUsers = _mapper.Map<IEnumerable<UserResponseDTO>>(users);

        return await Task.FromResult(mappedUsers); // for async task
        
    }

    public Task<UserResponseDTO> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(Guid id, CreateUserDTO createUserDTO)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(Guid id, UpdateUserDTO updateUserDTO)
    {
        throw new NotImplementedException();
    }
}
