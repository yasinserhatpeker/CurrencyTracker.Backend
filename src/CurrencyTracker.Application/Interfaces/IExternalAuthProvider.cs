using CurrencyTracker.Application.Models;

namespace CurrencyTracker.Application.Interfaces;

public interface IExternalAuthProvider
{
 Task<ExternalUserInfo> ValidateGoogleTokenAsync (string IdToken);
}
