using System;
using CurrencyTracker.Application.DTOs.Portfolios;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.PortfolioValidator;

public class CreatePortfolioDtoValidator  : AbstractValidator<CreatePortfolioDTO>
{
    public CreatePortfolioDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}
