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

        await _userRepository.SaveAsync();

    }

    public async Task RemoveUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if(user is null)
        {
            throw new Exception("User is not found");
        }
         _userRepository.Remove(user);

        await _userRepository.SaveAsync();
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        var users = _userRepository.GetAll();
        var mappedUsers = _mapper.Map<IEnumerable<UserResponseDTO>>(users);

        return await Task.FromResult(mappedUsers); // for async task
        
    }

    public async Task<UserResponseDTO> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if(user is null)
        {
            return null!;
        }
        return _mapper.Map<UserResponseDTO>(user);


    }

    public async Task UpdateUserAsync(Guid id, UpdateUserDTO updateUserDTO)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if(user is null)
        {
            return;

        }
        _mapper.Map(updateUserDTO,user);

        _userRepository.Update(user);

        await _userRepository.SaveAsync();

    }
}
