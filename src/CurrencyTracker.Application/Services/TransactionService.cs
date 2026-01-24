using CurrencyTracker.Domain.Entities;
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
        var transaction = _mapper.Map<Transaction>(createTransactionsDTO);
        
        await _transactionRepository.AddAsync(transaction);

        await _transactionRepository.SaveAsync();
    }

    public async Task RemoveTransactionAsync(Guid id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if(transaction is null)
        {
           throw new Exception("Transaction is not found");
        }
         _transactionRepository.Remove(transaction);

         await _transactionRepository.SaveAsync();
    }

    public async Task<TransactionResponseDTO> GetByIdAsync(Guid id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if(transaction is null)
        {
            throw new Exception("Transaction is not found");
        }
        return _mapper.Map<TransactionResponseDTO>(transaction);
    }

    public Task<IEnumerable<TransactionResponseDTO>> GetTransactionsByPortfolioAsync(Guid portfolioId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateTransactionAsync(Guid id, UpdateTransactionDTO updateTransactionDTO)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if(transaction is null)
        {
            throw new Exception("No transaction is found");
        }
         _mapper.Map(updateTransactionDTO,transaction);

         _transactionRepository.Update(transaction);
         
         await _transactionRepository.SaveAsync();
   }
}
