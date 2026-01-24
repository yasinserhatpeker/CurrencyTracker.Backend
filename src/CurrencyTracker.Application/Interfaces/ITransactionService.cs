using CurrencyTracker.Application.DTOs.Transactions;

namespace CurrencyTracker.Application.Interfaces;

public interface ITransactionService
{
    Task CreateTransactionAsync(CreateTransactionsDTO createTransactionsDTO);
    Task<IEnumerable<TransactionResponseDTO>> GetTransactionsByPortfolioAsync(Guid portfolioId);
    Task<TransactionResponseDTO> GetByIdAsync(Guid Id);
    Task DeleteTransactionAsync(Guid id);
    Task UpdateTransactionAsync(Guid id, UpdateTransactionDTO updateTransactionDTO);

}
