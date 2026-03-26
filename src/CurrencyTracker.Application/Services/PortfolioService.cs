using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CurrencyTracker.Application.DTOs.Portfolios;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens.Experimental;


namespace CurrencyTracker.Application.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Portfolio> _portfolioRepository;
    private readonly IGenericRepository<Transaction> _transactionRepository;
    private readonly ILogger<PortfolioService> _logger;
    private readonly IMarketService _marketService;

    public PortfolioService(IMapper mapper, IGenericRepository<Portfolio> portfolioRepository, ILogger<PortfolioService> logger, IGenericRepository<Transaction> transactionRepository, IMarketService marketService)
    {
        _mapper = mapper;
        _portfolioRepository = portfolioRepository;
        _logger = logger;
        _transactionRepository = transactionRepository;
        _marketService = marketService;
    }

    public async Task<PortfolioResponseDTO> CreatePortfolioAsync(CreatePortfolioDTO createPortfolioDTO)
    {
        var newPortfolio = _mapper.Map<Portfolio>(createPortfolioDTO);

        await _portfolioRepository.AddAsync(newPortfolio);

        _logger.LogInformation("a new portfolio is created for the user {UserId} and the id of the portfolio is {Id}", createPortfolioDTO.UserId, newPortfolio.Id);

        return _mapper.Map<PortfolioResponseDTO>(newPortfolio);


    }

    public async Task DeletePortfolioAsync(Guid id, Guid userId)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(id);
        if (portfolio is null || portfolio.UserId != userId)
        {
            _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id} and its user id is {UserId}", id, userId);
            throw new KeyNotFoundException("Portfolio not found");
        }
        var deletedPortfolio = await _portfolioRepository.DeleteAsync(id);

        _logger.LogInformation("a portfolio is deleted for the user {UserId} and the id of the portfolio is {Id}", deletedPortfolio.UserId, deletedPortfolio.Id);


    }

    public async Task<PortfolioResponseDTO> GetByIdAsync(Guid id)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(id);
        if (portfolio is null)
        {
            _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id}", id);
            throw new KeyNotFoundException("Portfolio not found");
        }
        return _mapper.Map<PortfolioResponseDTO>(portfolio);
    }

    public async Task<IEnumerable<PortfolioResponseDTO>> GetPortfoliosByUserAsync(Guid userId)
    {
        var userPortfolios = await _portfolioRepository.Find(x => x.UserId == userId);
        if (userPortfolios is null)
        {
            _logger.LogWarning("no portfolio is found for the user {UserId}", userId);
            throw new KeyNotFoundException("No portfolio is found for the user");
        }

        return _mapper.Map<IEnumerable<PortfolioResponseDTO>>(userPortfolios);

    }


    public async Task<PortfolioSummaryDTO> GetPortfolioSummaryAsync(Guid id, Guid userId)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(id);
        if (portfolio is null || portfolio.UserId != userId)
        {
            _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id} and its user id is {UserId}", id, userId);
            throw new KeyNotFoundException("Portfolio not found");
        }
        var transactions = await _transactionRepository.Find(x=>x.PortfolioId == id);
        
        var groupedTransactions = transactions
         .GroupBy(x=>x.BaseCurrency)
         .Select(g => new
         {
             BaseCurrency = g.Key,
             TotalQuantity = g.Sum(x => x.Quantity),
             TotalCost = g.Sum(x=>x.Price * x.Quantity),
         }).ToList();

        if (!groupedTransactions.Any())
        {
            _logger.LogWarning("no transaction is found for the portfolio {Id}", id);
            return new PortfolioSummaryDTO
            {
                Id = id,
                UserId = userId,
                Name = portfolio.Name,
                TotalInvested = 0,
                CurrentValue = 0
            };
        
        }
        decimal masterCurrentValue = 0;
        decimal masterTotalInvested = 0;

        foreach(var group in groupedTransactions)
        {
            masterTotalInvested += group.TotalCost;
            try
            {
            var currentPrice = await _marketService.GetMarketPriceAsync(group.BaseCurrency,portfolio.DisplayCurrency);

            masterCurrentValue += currentPrice.Price * group.TotalQuantity;
                
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get the market price for the base currency {BaseCurrency}", group.BaseCurrency);
                masterCurrentValue += group.TotalCost;
            }

        }
        return new PortfolioSummaryDTO
        {
            Id = id,
            UserId = userId,
            Name = portfolio.Name,
            TotalInvested = masterTotalInvested,
            CurrentValue = masterCurrentValue
        };

    }

    public async Task<PortfolioResponseDTO> UpdatePortfolioAsync(Guid id, UpdatePortfolioDTO updatePortfolioDTO, Guid userId)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(id);
        if (portfolio is null || portfolio.UserId != userId)
        {
            _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id}", id);
            throw new KeyNotFoundException("No portfolio is found");

        }
        var oldName = portfolio.Name;

        _mapper.Map(updatePortfolioDTO, portfolio);
        await _portfolioRepository.UpdateAsync(portfolio);

        _logger.LogInformation("a portfolio is updated. OldName={OldName}, NewName={NewName} and its UserId={UserId}", oldName, portfolio.Name, portfolio.UserId);

        return _mapper.Map<PortfolioResponseDTO>(portfolio);

    }




}
