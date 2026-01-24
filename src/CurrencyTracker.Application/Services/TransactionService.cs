using System;
using CurrencyTracker.Application.DTOs.Transactions;
using CurrencyTracker.Application.Interfaces;

namespace CurrencyTracker.Application.Services;

public class TransactionService : ITransactionService
{
    public Task CreateTransactionAsync(CreateTransactionsDTO createTransactionsDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTransactionAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TransactionResponseDTO> GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TransactionResponseDTO>> GetTransactionsByPortfolioAsync(Guid portfolioId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTransactionAsync(Guid id, UpdateTransactionDTO updateTransactionDTO)
    {
        throw new NotImplementedException();
    }
}
