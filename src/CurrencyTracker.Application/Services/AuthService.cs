using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;
using CurrencyTracker.Domain.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace CurrencyTracker.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IMapper mapper, IGenericRepository<User> userRepository, IConfiguration configuration)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _configuration = configuration;
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

        await _userRepository.AddAsync(user);
        return _mapper.Map<UserResponseDTO>(user);

    }

    
    public async Task<AuthResponseDTO> LoginAsync(LoginUserDTO loginUserDTO)
    {
        var users = await _userRepository.Find(u => u.Email == loginUserDTO.Email);
        var user = users.FirstOrDefault();

        if (user is null || user.PasswordHash is null || !BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.PasswordHash))
        {
            throw new KeyNotFoundException("Invalid email or password.");
        }
        return await GenerateAuthResponseAsync(user); // helper method for less code
    }

    public async Task<AuthResponseDTO> RefreshTokenAsync(string RefreshToken)
    {
        var users = await _userRepository.Find(u => u.RefreshToken == RefreshToken);
        var user = users.FirstOrDefault();

        if (user is null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            throw new KeyNotFoundException("The session is expired. Please try again");
        }
        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponseDTO> GenerateAuthResponseAsync(User user)
    {
        var accesToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // 7-day refresh token

        await _userRepository.UpdateAsync(user);

        return new AuthResponseDTO
        {
            AccessToken = accesToken,
            RefreshToken = refreshToken
        };

    }
    private string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
          new Claim(JwtRegisteredClaimNames.Sub ,user.Id.ToString()),
          new Claim(JwtRegisteredClaimNames.Email, user.Email),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // token-refreshing with token ID

        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10), // 10-minute access token
            signingCredentials: creds
  );
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);

        }
    }

    public async Task<AuthResponseDTO> GoogleLoginAsync(GoogleLoginDTO googleLoginDTO)
    {
        GoogleJsonWebSignature.Payload payload;

        try
        {
            var clientId = _configuration["GoogleAuthSettings:ClientId"]; // get the clientId from configuration
            
            var settings = new GoogleJsonWebSignature.ValidationSettings() 
            {
                Audience = new List<string>() {clientId!}  // we get the clientId for auth
            };

            payload = await GoogleJsonWebSignature.ValidateAsync(googleLoginDTO.IdToken,settings); 
            // validating the token 

            // if invalid throw a exception

        } 
        catch(InvalidJwtException)
        {
            throw new KeyNotFoundException("Invalid Google token");
        }

        var users = await _userRepository.Find(u => u.Email == payload.Email);
        var existingUser = users.FirstOrDefault(); // check if the user exists in the DB

        if(existingUser!=null)
        {
            return await GenerateAuthResponseAsync(existingUser); // returns the existing user to generate token
        }

        // if there's no user create one
        else
        {
            var newUser = new User
            {   Id = Guid.NewGuid(),     // random Guid ID
                Email = payload.Email,   // using the Google email
                Username = payload.Name, // using the Google name e.g "Y. Serhat Peker"
                AuthProvider = "Google",  // mark them as a google user
            
               // generate a random strong password hash because DB use it
               // they never use this password, they will use google
               PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
               CreatedAt = DateTime.UtcNow,
            };

            await _userRepository.AddAsync(newUser);
            return await GenerateAuthResponseAsync(newUser); // generate token for new user
           

        }



    }
}
