using System;
using CurrencyTracker.Application.DTOs.Transactions;

namespace CurrencyTracker.Application.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionResponseDTO>> GetAllTransactionsByUser(Guid userId);
}
