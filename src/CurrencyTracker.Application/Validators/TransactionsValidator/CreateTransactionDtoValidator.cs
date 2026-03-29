using CurrencyTracker.Application.DTOs.Transactions;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.TransactionsValidator;

public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDTO>
{
    public CreateTransactionDtoValidator()
    {
        RuleFor(x => x.PortfolioId).NotEmpty();
        RuleFor(x => x.TransactionDate).NotEmpty();
        RuleFor(x => x.BaseCurrency).NotEmpty().MaximumLength(10).MinimumLength(2);
        RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
        RuleFor(x => x.TransactionType).NotEmpty().IsInEnum();
        RuleFor(x => x.QuoteCurrency).NotEmpty().Length(3).WithMessage("Quote Currency must be exactly 3 characters e.g, USD, EUR, GBP, TRY...");


    }
}
