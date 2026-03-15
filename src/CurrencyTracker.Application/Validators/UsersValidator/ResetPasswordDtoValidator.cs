using System;
using CurrencyTracker.Application.DTOs.Users;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.UsersValidator;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDTO>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(x => x.ResetPasswordToken).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
        RuleFor(x=>x.ConfirmPassword).NotEmpty().Equal(x=>x.NewPassword).WithMessage("Passwords do not match");
        

    }
}
