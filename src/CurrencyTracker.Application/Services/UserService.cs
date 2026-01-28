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

    public async Task DeleteUserAsync(Guid id)
    {
        var deletedUser = await _userRepository.DeleteAsync(id);
        if(deletedUser is null)
        {
            throw new Exception("User not found");
        }

       
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
   
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
        
    }

    public async Task<UserResponseDTO> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if(user is null)
        {
            throw new Exception("No user is found");
        }
        return _mapper.Map<UserResponseDTO>(user);


    }

    public async Task UpdateUserAsync(Guid id, UpdateUserDTO updateUserDTO)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if(user is null)
        {
             throw new Exception("No user is found");

        }
        _mapper.Map(updateUserDTO,user);

        await _userRepository.UpdateAsync(user);


    }
}
