using System;
using CurrencyTracker.Application.DTOs.Transactions;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.TransactionsValidator;
public class UpdateTransactionDtoValidator : AbstractValidator<UpdateTransactionDTO>
{
    public UpdateTransactionDtoValidator()
    {
        RuleFor(x => x.TransactionType).NotEmpty().IsInEnum();
        RuleFor(x => x.Symbol).NotEmpty().MaximumLength(10).MinimumLength(2).WithMessage("Symbol must be between 10 and 2 characters e.g, ");
        RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
        RuleFor(x => x.TransactionDate).NotEmpty();
        RuleFor(x => x.QuoteCurrency).NotEmpty().Length(3).WithMessage("Quote Currency must be exactly 3 characters e.g, USD, EUR, GBP, TRY...");
    }
}
