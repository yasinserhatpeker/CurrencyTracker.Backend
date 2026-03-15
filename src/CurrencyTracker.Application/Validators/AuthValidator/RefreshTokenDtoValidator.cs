using System;
using CurrencyTracker.Application.DTOs.Auth;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.AuthValidator;

public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDTO>
{
    public RefreshTokenDtoValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
