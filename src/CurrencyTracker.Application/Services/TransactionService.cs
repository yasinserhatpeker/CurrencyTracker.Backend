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

       
    }

    public async Task DeleteTransactionAsync(Guid id)
    {
        var deletedTransaction =await _transactionRepository.DeleteAsync(id);
        if(deletedTransaction is null)
        {
            throw new Exception("Transaction not found");
        }

        
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

    public async Task<IEnumerable<TransactionResponseDTO>> GetTransactionsByPortfolioAsync(Guid portfolioId)
    {
      var portfolioTransactions = await _transactionRepository.Find(x=>x.PortfolioId==portfolioId);
 
       return _mapper.Map<IEnumerable<TransactionResponseDTO>>(portfolioTransactions);


    }

    public async Task UpdateTransactionAsync(Guid id, UpdateTransactionDTO updateTransactionDTO)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if(transaction is null)
        {
            throw new Exception("No transaction is found");
        }
         _mapper.Map(updateTransactionDTO,transaction);

        await  _transactionRepository.UpdateAsync(transaction);

         
   }
}
