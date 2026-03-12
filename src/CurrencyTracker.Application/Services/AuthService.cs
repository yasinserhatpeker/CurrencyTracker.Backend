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
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace CurrencyTracker.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;

    private readonly IExternalAuthProvider _externalAuthProvider;

    public AuthService(IMapper mapper, IGenericRepository<User> userRepository, IConfiguration configuration, IEmailService emailService, IGenericRepository<RefreshToken> refreshTokenRepository,IExternalAuthProvider externalAuthProvider)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _configuration = configuration;
        _emailService = emailService;
        _refreshTokenRepository = refreshTokenRepository;
        _externalAuthProvider=externalAuthProvider;
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

        var token = GenerateSecureToken(); // generating token with randomNumberGenerator
        user.EmailVerificationTokenHash = HashToken(token); // hashing with SHA256
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
            throw new KeyNotFoundException("Email or password is invalid");
        }
        if(!user.IsEmailVerified)
        {
            throw new UnauthorizedAccessException("Please verify your email before logging in.");
        }
        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO)
    {   
        var hashed = HashToken(refreshTokenDTO.RefreshToken);
        var refreshTokens = await _refreshTokenRepository.Find(u=>u.HashToken == hashed);
        var refreshToken = refreshTokens.FirstOrDefault();

        if (refreshToken is null || refreshToken.ExpiryTime is null || refreshToken.ExpiryTime < DateTime.UtcNow)
        {
            throw new KeyNotFoundException("The session is expired. Please try again");
        }

        var user = await _userRepository.GetByIdAsync(refreshToken.UserId);
        if(user is null)
        {
            throw new KeyNotFoundException("Cannot retrieve user");
        }

        await _refreshTokenRepository.DeleteAsync(refreshToken.Id);
         
        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponseDTO> GenerateAuthResponseAsync(User user)
    {
        var accesToken = GenerateAccessToken(user);
        var refreshToken = GenerateSecureToken();

        var newSession = new RefreshToken
        {
            Id = Guid.NewGuid(), // creating new guid Id for refresh token
            HashToken=HashToken(refreshToken), // hashing the refresh token with SHA256
            ExpiryTime=DateTime.UtcNow.AddDays(7), // expiry time
            UserId = user.Id // connecting with the userId
           
        };

        await _refreshTokenRepository.AddAsync(newSession);

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
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(JwtRegisteredClaimNames.Email, user.Email),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // token-refreshing with token ID

        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
        (
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10), // 10-minute access token
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
    private string GenerateSecureToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Base64UrlEncoder.Encode(randomNumber);

        }
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
            return await GenerateAuthResponseAsync(existingUser);
        }

    

    public async Task LogoutAsync(RefreshTokenDTO refreshTokenDTO)
    {
     
       var hashed = HashToken(refreshTokenDTO.RefreshToken);
       
      
       var refreshTokens = await _refreshTokenRepository.Find(u => u.HashToken == hashed);
       var refreshToken = refreshTokens.FirstOrDefault();

       if(refreshToken is not null)
        {
            await _refreshTokenRepository.DeleteAsync(refreshToken.Id);
        }


    }
    private string HashToken(string token)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO)
    {
       var users = await _userRepository.Find(u=>u.Email == forgotPasswordDTO.Email);
       var user = users.FirstOrDefault();
       if(user is null)
        {
           return; // returning silently so email enumeration attacks can be prevented
        }
        var token = GenerateSecureToken(); // generating 32-byte token with RandomNumberGenerator
        var hashedToken = HashToken(token); // hashing the token with SHA256

        user.ResetPasswordTokenHash = hashedToken;  // implementing DB
        user.ResetPasswordTokenExpiryTime = DateTime.UtcNow.AddMinutes(15);

        await _userRepository.UpdateAsync(user);

        var resetLink = $"http://localhost:5172/api/auth/reset-password?token={token}"; // reset-link to reset your password

        await _emailService.SendEmailAsync(
            to:user.Email,
            subject:"Password reset request",
            body:$"Click the link to reset your password:{resetLink}"
        ); 
    }

    public async Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
    {
        var hashed = HashToken(resetPasswordDTO.ResetPasswordToken); // fetching hashed token
        var users = await _userRepository.Find(u=>u.ResetPasswordTokenHash == hashed); // checking if the token hashed
        var user = users.FirstOrDefault();
        
        if(user is null || user.ResetPasswordTokenExpiryTime < DateTime.UtcNow)
        {
            throw new Exception("Invalid or expired password reset token");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.NewPassword); // hashing the new password with BCrypt 

        user.ResetPasswordTokenHash=null; // preventing token reuse
        user.ResetPasswordTokenExpiryTime=null;

        await _userRepository.UpdateAsync(user);
        
    }

    public async Task<bool> EmailVerificationAsync(string token)
    {
        var hashed = HashToken(token);
        var users = await _userRepository.Find(u=>u.EmailVerificationTokenHash == hashed);
        var user = users.FirstOrDefault();
        if(user is null)
        {
            return false;
        }
        user.IsEmailVerified=true;
        user.EmailVerificationTokenHash=null;
        await _userRepository.UpdateAsync(user);
        return true;
        
    }

  
}
