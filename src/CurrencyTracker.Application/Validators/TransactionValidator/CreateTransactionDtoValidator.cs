using System;
using CurrencyTracker.Application.DTOs.Transactions;
using FluentValidation;

namespace CurrencyTracker.Application.Validators.TransactionValidator;

public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionsDTO>
{
  public CreateTransactionDtoValidator()
    {
        
    }
}
