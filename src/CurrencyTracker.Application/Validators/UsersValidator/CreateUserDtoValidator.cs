using System;
using CurrencyTracker.Application.DTOs;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.UsersValidator;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDTO>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
