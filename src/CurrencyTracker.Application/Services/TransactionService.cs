using System;
using System.Transactions;
using AutoMapper;
using CurrencyTracker.Application.DTOs.Transactions;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Interfaces;

namespace CurrencyTracker.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Transaction> _transactionRepository;

    public TransactionService(IMapper mapper, IGenericRepository<Transaction> transactionRepository)
    {
        _mapper=mapper;
        _transactionRepository=transactionRepository;
    }
    public async Task CreateTransactionAsync(CreateTransactionsDTO createTransactionsDTO)
    {
        
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
