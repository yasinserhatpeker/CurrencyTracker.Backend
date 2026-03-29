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
    private readonly IMarketService _marketService;
    private readonly IGenericRepository<Portfolio> _portfolioRepository;

    public TransactionService(IMapper mapper, IGenericRepository<Transaction> transactionRepository, ILogger<TransactionService> logger, IMarketService marketService, IGenericRepository<Portfolio> portfolioRepository)
    {
        _mapper = mapper;
        _transactionRepository = transactionRepository;
        _logger = logger;
        _marketService = marketService;
        _portfolioRepository = portfolioRepository;
    }
    public async Task<TransactionResponseDTO> CreateTransactionAsync(CreateTransactionDTO createTransactionDTO, Guid userId)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(createTransactionDTO.PortfolioId);
        if (portfolio is null || portfolio.UserId != userId)
        {
            _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id} and its user id is {UserId}", createTransactionDTO.PortfolioId, userId);
            throw new KeyNotFoundException("Transaction's portfolio not found");
        }

        var marketData = await _marketService.GetMarketPriceAsync(createTransactionDTO.BaseCurrency, createTransactionDTO.QuoteCurrency);

        var transaction = _mapper.Map<Transaction>(createTransactionDTO);
        transaction.Price = marketData.Price;
        transaction.TransactionDate = DateTime.UtcNow;

        await _transactionRepository.AddAsync(transaction);

        await _transactionRepository.SaveAsync();

        _logger.LogInformation("Transaction {TransactionId} created for BaseCurrency {BaseCurrency} via {Provider}", transaction.Id, transaction.BaseCurrency, marketData.Source);


        return _mapper.Map<TransactionResponseDTO>(transaction);
    }

    public async Task DeleteTransactionAsync(Guid id,Guid userId)
    {
        var deletedTransaction = await _transactionRepository.DeleteAsync(id);
        var portfolio = await _portfolioRepository.GetByIdAsync(deletedTransaction!.PortfolioId);
        if(portfolio is null || portfolio.UserId != userId)
        {
            _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id} and its user id is {UserId}", deletedTransaction.PortfolioId, userId);
            throw new KeyNotFoundException("You dont have an access to do it");
        }
        if (deletedTransaction is null)
        {
            _logger.LogWarning("a transaction is not found. The id of the transaction is {Id}", id);
            throw new KeyNotFoundException("Transaction not found");
        }
        _logger.LogInformation("a transaction is deleted for the portfolio {PortfolioId} and the id of the transaction is {Id}", deletedTransaction.PortfolioId, deletedTransaction.Id);


    }

    public async Task<TransactionResponseDTO> GetByIdAsync(Guid id, Guid userId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        var portfolio = await _portfolioRepository.GetByIdAsync(transaction!.PortfolioId);
        if(portfolio is null || portfolio.UserId != userId)
        {   
            _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id} and its user id is {UserId}", transaction.PortfolioId, userId);
            throw new KeyNotFoundException("You dont have an access to do it");
        }
        if (transaction is null)
        {
            throw new KeyNotFoundException("Transaction is not found");
        }
        return _mapper.Map<TransactionResponseDTO>(transaction);
    }

    public async Task<IEnumerable<TransactionResponseDTO>> GetTransactionsByPortfolioAsync(Guid portfolioId)
    {
        var portfolioTransactions = await _transactionRepository.Find(x => x.PortfolioId == portfolioId);

        if (portfolioTransactions is null | !portfolioTransactions!.Any())
        {
            _logger.LogWarning("no transaction is found for the portfolio {PortfolioId}", portfolioId);

            return Enumerable.Empty<TransactionResponseDTO>();
        }


        return _mapper.Map<IEnumerable<TransactionResponseDTO>>(portfolioTransactions);

    }
    public async Task<TransactionResponseDTO> UpdateTransactionAsync(Guid id, UpdateTransactionDTO updateTransactionDTO, Guid userId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if (transaction is null)
        {
            _logger.LogWarning("a transaction is not found. The id of the transaction is {Id}", id);
            throw new KeyNotFoundException("Transaction not found");
        }
        var portfolio = await _portfolioService.GetByIdAsync(transaction.PortfolioId);

        if (portfolio is null || portfolio.UserId != userId)
        {
            throw new KeyNotFoundException("You dont have an access to do it");
        }

        var oldPrice = transaction.Price;
        var oldQuantity = transaction.Quantity;

        _mapper.Map(updateTransactionDTO, transaction);

        await _transactionRepository.UpdateAsync(transaction);
        await _transactionRepository.SaveAsync();

        _logger.LogInformation("AUDIT: Transaction {Id} updated. Price: {OldP} -> {NewP}, Qty: {OldQ} -> {NewQ} and transaction date {Date}",
        transaction.Id, oldPrice, transaction.Price, oldQuantity, transaction.Quantity, transaction.TransactionDate);

        return _mapper.Map<TransactionResponseDTO>(transaction);

    }
}

