using System;
using CurrencyTracker.Application.DTOs.Portfolios;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.PortfoliosValidator;

public class UpdatePortfolioDtoValidator : AbstractValidator<UpdatePortfolioDTO>
{
   public UpdatePortfolioDtoValidator()
    {
        RuleFor(x=>x.Name).NotEmpty().MaximumLength(50);
    }
}
