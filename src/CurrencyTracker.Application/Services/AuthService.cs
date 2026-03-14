using AutoMapper;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Helpers;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;


namespace CurrencyTracker.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;
    private readonly IExternalAuthProvider _externalAuthProvider;

    public AuthService(IMapper mapper, IGenericRepository<User> userRepository, IEmailService emailService,IExternalAuthProvider externalAuthProvider, ITokenService tokenService)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _emailService = emailService;
        _externalAuthProvider=externalAuthProvider;
        _tokenService=tokenService;
    }
    public async Task<UserResponseDTO> RegisterAsync(CreateUserDTO createUserDTO)
    {
        var existingUsers = await _userRepository.Find(u => u.Email == createUserDTO.Email);
        if (existingUsers.Any())
        {
            throw new KeyNotFoundException("This email is already used");
        }
        var user = _mapper.Map<User>(createUserDTO);

        //password hashing
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDTO.Password);
        user.AuthProvider = "Local";

        var token = CryptoHelpers.GenerateSecureToken(); // generating token with randomNumberGenerator
        user.EmailVerificationTokenHash = CryptoHelpers.HashToken(token); // hashing with SHA256
        user.IsEmailVerified = false; // ensure they are locked out until they verify

        await _userRepository.AddAsync(user); // adding to the DB
         
        var verificationLink =$"http://localhost/api/auth/verify-email?token={token}"; // verification-link

        await _emailService.SendEmailAsync(
            to:user.Email,
            subject:"Email verification",
            body:$"Click the link to verify your email:{verificationLink}"

        );

        return _mapper.Map<UserResponseDTO>(user); // mapping with autoMapper

    }

    
    public async Task<AuthResponseDTO> LoginAsync(LoginUserDTO loginUserDTO)
    {
        var users = await _userRepository.Find(u=>u.Email == loginUserDTO.Email);
        var user = users.FirstOrDefault();

        if(user is null || user.PasswordHash is null || !BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.PasswordHash)) 
        {
            throw new UnauthorizedAccessException("Email or password is invalid");
        }
        if(!user.IsEmailVerified)
        {
            throw new UnauthorizedAccessException("Please verify your email before logging in.");
        }
        return await _tokenService.GenerateAuthResponseAsync(user);
    }


    public async Task<AuthResponseDTO> GoogleLoginAsync(GoogleLoginDTO googleLoginDTO)
    {
       var externalUser = await _externalAuthProvider.ValidateGoogleTokenAsync(googleLoginDTO.IdToken);
       if(!externalUser.IsEmailVerified)
        {
            throw new Exception("Please verify your Google email");
        }
        var users = await _userRepository.Find(u=>u.GoogleId == externalUser.ProviderUserId);
        var existingUser = users.FirstOrDefault();

        if(existingUser is null)
        {
            existingUser = (await _userRepository.Find(u=>u.Email == externalUser.Email)).FirstOrDefault();

            if(existingUser is not null)
            {
                existingUser.GoogleId = externalUser.ProviderUserId;
                await _userRepository.UpdateAsync(existingUser);
            }
         }
        if(existingUser is null)
            {
                existingUser = new User
                {
                    Id=Guid.NewGuid(),
                    Email=externalUser.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                    Username = externalUser.Name,
                    GoogleId=externalUser.ProviderUserId,
                    AuthProvider="Google",
                    IsEmailVerified =externalUser.IsEmailVerified,
                    CreatedAt = DateTime.UtcNow
                };

            await _userRepository.AddAsync(existingUser);
            }
            return await _tokenService.GenerateAuthResponseAsync(existingUser);
        }


}
