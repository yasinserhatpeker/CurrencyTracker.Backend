using System;
using CurrencyTracker.Application.DTOs.Users;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.UsersValidator;

public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDTO>
{
    public ForgotPasswordDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}

