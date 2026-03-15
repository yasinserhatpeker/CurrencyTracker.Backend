using System;
using CurrencyTracker.Application.DTOs.Users;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.UsersValidator;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x=>x.Username).NotEmpty();
    }
}
