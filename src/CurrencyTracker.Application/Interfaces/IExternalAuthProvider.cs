using System;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.Models;

namespace CurrencyTracker.Application.Interfaces;

public interface IExternalAuthProvider
{
    Task<ExternalUserInfo> GoogleLoginAsync (string IdToken);
}
