using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Helpers;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;


namespace CurrencyTracker.Application.Services;

public class UserAccountService :IUserAccountService
{
  
   private readonly IGenericRepository<User> _userRepository;
   private readonly IEmailService _emailService;
    
    public UserAccountService(IGenericRepository<User> userRepository,IEmailService emailService)
    {
       
        _userRepository = userRepository;
        _emailService = emailService;

    }

    public async Task<bool> EmailVerificationAsync(string token)
    {
        var hashed = CryptoHelpers.HashToken(token);
        var users = await _userRepository.Find(u=>u.EmailVerificationTokenHash == hashed);
        var user = users.FirstOrDefault();

        if(user is null)
        {
            return false;
        }
        user.IsEmailVerified = true;
        user.EmailVerificationTokenHash=null;
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO)
    {
        var users = await _userRepository.Find(u=>u.Email == forgotPasswordDTO.Email);
        var user = users.FirstOrDefault();

        if(user is null)
        {
             return; // returning silently so email enumeration attacks can be prevented
        }

        var token = CryptoHelpers.GenerateSecureToken();  // generating 32-byte token with RandomNumberGenerator
        var hashed = CryptoHelpers.HashToken(token);   // hashing the token with SHA256

        user.ResetPasswordTokenHash = hashed;
        user.ResetPasswordTokenExpiryTime = DateTime.UtcNow.AddMinutes(15);

        await _userRepository.UpdateAsync(user);

        var resetLink =$"http://localhost:5172/api/auth/reset-password?token={token}"; // reset-link to reset your password

        await _emailService.SendEmailAsync(
            to:user.Email,
            subject:"Password reset request",
            body:$"Click the link to reset your password:{resetLink}"
        );

    }

    public async Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
    {
        var token = CryptoHelpers.HashToken(resetPasswordDTO.ResetPasswordToken);
        var users = await _userRepository.Find(u=>u.ResetPasswordTokenHash == token);
        var user = users.FirstOrDefault();

        if(user is null || user.ResetPasswordTokenExpiryTime < DateTime.UtcNow)
        {
            throw new KeyNotFoundException("Invalid or expired reset password token");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.NewPassword);

        user.ResetPasswordTokenHash=null;
        user.ResetPasswordTokenExpiryTime = null; //prevent token reuse

        await _userRepository.UpdateAsync(user);

    }
}
