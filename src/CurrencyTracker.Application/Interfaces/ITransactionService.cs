using CurrencyTracker.Application.DTOs.Transactions;

namespace CurrencyTracker.Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponseDTO> CreateTransactionAsync(CreateTransactionDTO createTransactionDTO,Guid userId);
    Task<IEnumerable<TransactionResponseDTO>> GetTransactionsByPortfolioAsync(Guid portfolioId);
    Task<TransactionResponseDTO> GetByIdAsync(Guid Id,Guid userId);
    Task DeleteTransactionAsync(Guid id);
    Task<TransactionResponseDTO> UpdateTransactionAsync(Guid id, UpdateTransactionDTO updateTransactionDTO,Guid userId);

}
