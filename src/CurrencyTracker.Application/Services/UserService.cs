using AutoMapper;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;
using Microsoft.Extensions.Logging;


namespace CurrencyTracker.Application.Services;

public class UserService : IUserService
{  
    private readonly IMapper _mapper;
    private readonly IGenericRepository<User> _userRepository;
    
    private readonly ILogger<UserService> _logger;
    public UserService(IMapper mapper, IGenericRepository<User> userRepository, ILogger<UserService> logger)
    {
        _mapper=mapper;
        _userRepository=userRepository;
        _logger = logger;
    }


     
    public async Task<UserResponseDTO> CreateUserAsync(CreateUserDTO createUserDTO)
    {
        var newUser = _mapper.Map<User>(createUserDTO);
        await _userRepository.AddAsync(newUser);

        _logger.LogInformation("New user is created. Id={Id}",newUser.Id);
        return _mapper.Map<UserResponseDTO>(newUser);

       
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var deletedUser = await _userRepository.DeleteAsync(id);
        if(deletedUser is null)
        {   
            _logger.LogWarning("a user is not found. The id of the user is {Id}", id);
            throw new KeyNotFoundException("User not found");
        }
        _logger.LogInformation("User is deleted. Id={Id}",deletedUser.Id);

       
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
            _logger.LogWarning("a user is not found. The id of the user is {Id}", id);
            throw new KeyNotFoundException("No user is found");
        }
        return _mapper.Map<UserResponseDTO>(user);


    }

    public async Task<UserResponseDTO> UpdateUserAsync(Guid id, UpdateUserDTO updateUserDTO)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if(user is null)
        {    
            _logger.LogWarning("a user is not found. The id of the user is {Id}", id); 
             throw new KeyNotFoundException("No user is found");

        }
       
        
        _mapper.Map(updateUserDTO,user);

        await _userRepository.UpdateAsync(user);
        _logger.LogInformation("User is updated. UserId={UserId}",user.Id);

        return _mapper.Map<UserResponseDTO>(user);


    }
}
