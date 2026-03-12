using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CurrencyTracker.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyTracker.Application.Helpers;

public static class CryptoHelpers
{    
    public static string GenerateSecureToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Base64UrlEncoder.Encode(randomNumber);
        }
    }

    public static string HashToken(string token)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);

    }
}
