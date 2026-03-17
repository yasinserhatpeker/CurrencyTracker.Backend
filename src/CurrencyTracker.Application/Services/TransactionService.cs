using CurrencyTracker.Domain.Entities;
using AutoMapper;
using CurrencyTracker.Application.DTOs.Transactions;
using CurrencyTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;


namespace CurrencyTracker.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Transaction> _transactionRepository;

    private readonly ILogger<TransactionService> _logger;

    public TransactionService(IMapper mapper, IGenericRepository<Transaction> transactionRepository,ILogger<TransactionService> logger)
    {
        _mapper=mapper;
        _transactionRepository=transactionRepository;
        _logger = logger;
    }
    public async Task<TransactionResponseDTO> CreateTransactionAsync(CreateTransactionsDTO createTransactionsDTO)
    {
        var transaction = _mapper.Map<Transaction>(createTransactionsDTO);
        
         await _transactionRepository.AddAsync(transaction);

         _logger.LogInformation("new transaction created for the portfolio : {PortfolioId} and the id of the transaction is {Id}, the Currency is {QuoteCurrency}, the symbol is {Symbol}, the price is {Price}, the quantity is {Quantity}", transaction.PortfolioId, transaction.Id, transaction.QuoteCurrency, transaction.Symbol, transaction.Price, transaction.Quantity);

        return _mapper.Map<TransactionResponseDTO>(transaction);

    }

    public async Task DeleteTransactionAsync(Guid id)
    {
        var deletedTransaction =await _transactionRepository.DeleteAsync(id);
        if(deletedTransaction is null)
        {   
            _logger.LogWarning("a transaction is not found. The id of the transaction is {Id}", id);
            throw new KeyNotFoundException("Transaction not found");
        }
        _logger.LogInformation("a transaction is deleted for the portfolio {PortfolioId} and the id of the transaction is {Id}", deletedTransaction.PortfolioId, deletedTransaction.Id);

        
    }

    public async Task<TransactionResponseDTO> GetByIdAsync(Guid id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if(transaction is null)
        {
            throw new KeyNotFoundException("Transaction is not found");
        }
        return _mapper.Map<TransactionResponseDTO>(transaction);
    }

    public async Task<IEnumerable<TransactionResponseDTO>> GetTransactionsByPortfolioAsync(Guid portfolioId)
    {
      var portfolioTransactions = await _transactionRepository.Find(x=>x.PortfolioId==portfolioId);
 
       return _mapper.Map<IEnumerable<TransactionResponseDTO>>(portfolioTransactions);


    }

    public async Task<TransactionResponseDTO> UpdateTransactionAsync(Guid id, UpdateTransactionDTO updateTransactionDTO)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if(transaction is null)
        {   
            _logger.LogWarning("a transaction is not found. The id of the transaction is {Id}", id);
            throw new KeyNotFoundException("No transaction is found");
        }

         var oldPrice = transaction.Price;
         var oldQuantity = transaction.Quantity;

         _mapper.Map(updateTransactionDTO,transaction);

        await  _transactionRepository.UpdateAsync(transaction);

        _logger.LogInformation("AUDIT: Transaction updated. Id={Id}, OldPrice={OldPrice}, OldQuantity={OldQuantity}, NewPrice={NewPrice}, NewQuantity={NewQuantity}", transaction.Id, oldPrice, oldQuantity, transaction.Price, transaction.Quantity);

        return _mapper.Map<TransactionResponseDTO>(transaction);

         
   }
}
