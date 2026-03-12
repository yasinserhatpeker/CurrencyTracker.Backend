using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Models;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace CurrencyTracker.Infrastructure.Authentication;

public class GoogleAuthProvider : IExternalAuthProvider
{  
    private readonly IConfiguration _configuration;

    public GoogleAuthProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<ExternalUserInfo> ValidateGoogleTokenAsync(string IdToken)
    {
        var clientId = _configuration["GoogleAuthSettings:ClientId"];
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> {clientId!}
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(IdToken,settings);

        return new ExternalUserInfo
        {
            Email=payload.Email,
            Name=payload.Name,
            ProviderUserId = payload.Subject,
            IsEmailVerified=payload.EmailVerified
        };

    }
}
