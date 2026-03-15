using System;
using CurrencyTracker.Application.DTOs.Users;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.UsersValidator;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDTO>
{
    public LoginUserDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
