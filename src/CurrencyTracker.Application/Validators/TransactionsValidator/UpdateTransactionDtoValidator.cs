using System;
using CurrencyTracker.Application.DTOs.Transactions;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.TransactionsValidator;
public class UpdateTransactionDtoValidator : AbstractValidator<UpdateTransactionDTO>
{
    public UpdateTransactionDtoValidator()
    {
        RuleFor(x => x.TransactionType).NotEmpty().IsInEnum();
        RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
        RuleFor(x => x.TransactionDate).NotEmpty();
        
    }
}
