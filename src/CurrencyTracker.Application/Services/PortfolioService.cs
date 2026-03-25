using AutoMapper;
using CurrencyTracker.Application.DTOs.Portfolios;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;
using Microsoft.Extensions.Logging;


namespace CurrencyTracker.Application.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Portfolio> _portfolioRepository;
    private readonly IGenericRepository<Transaction> _transactionRepository;
    private readonly ILogger<PortfolioService> _logger;

    public PortfolioService(IMapper mapper, IGenericRepository<Portfolio> portfolioRepository, ILogger<PortfolioService> logger, IGenericRepository<Transaction> transactionRepository)
    {
        _mapper = mapper;
        _portfolioRepository = portfolioRepository;
        _logger = logger;
        _transactionRepository = transactionRepository;
    }

    public async Task<PortfolioResponseDTO> CreatePortfolioAsync(CreatePortfolioDTO createPortfolioDTO)
    {
        var newPortfolio = _mapper.Map<Portfolio>(createPortfolioDTO);

        await _portfolioRepository.AddAsync(newPortfolio);

        _logger.LogInformation("a new portfolio is created for the user {UserId} and the id of the portfolio is {Id}", createPortfolioDTO.UserId, newPortfolio.Id);

        return _mapper.Map<PortfolioResponseDTO>(newPortfolio);


    }

    public async Task DeletePortfolioAsync(Guid id)
    {
        var deletedPortfolio = await _portfolioRepository.DeleteAsync(id);
        if (deletedPortfolio is null)
        {
            _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id}", id);
            throw new KeyNotFoundException("Portfolio not found");
        }
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


    public async Task<PortfolioSummaryDTO> GetPortfolioSummaryAsync(Guid id,Guid userId)
    {
       var portfolio = await _portfolioRepository.GetByIdAsync(id);
       if(portfolio is null || portfolio.UserId != userId)
        {
            _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id} and its UserId:{UserId}", id,portfolio!.UserId);
            throw new KeyNotFoundException("Portfolio not found");
        }
        var transactions = await _transactionRepository.Find(x=>x.PortfolioId==id);
        if(transactions is null || !transactions.Any())
        {
            _logger.LogWarning("no transaction is found for the portfolio {PortfolioId}", id);
            throw new KeyNotFoundException("No transaction is found for the portfolio");
        }
        
        
        

    }

    public async Task<PortfolioResponseDTO> UpdatePortfolioAsync(Guid id, UpdatePortfolioDTO updatePortfolioDTO)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(id);
        if (portfolio is null)
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
