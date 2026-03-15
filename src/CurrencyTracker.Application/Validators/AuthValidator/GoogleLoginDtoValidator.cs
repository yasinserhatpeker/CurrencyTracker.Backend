using System;
using CurrencyTracker.Application.DTOs.Auth;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.AuthValidator;

public class GoogleLoginDtoValidator : AbstractValidator<GoogleLoginDTO>
{
    public GoogleLoginDtoValidator()
    {
        RuleFor(x => x.IdToken).NotEmpty();
    }
}
