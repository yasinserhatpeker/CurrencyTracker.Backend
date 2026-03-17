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

    private readonly ILogger<PortfolioService> _logger;

    public PortfolioService(IMapper mapper, IGenericRepository<Portfolio> portfolioRepository, ILogger<PortfolioService> logger)
    {
        _mapper=mapper;
        _portfolioRepository= portfolioRepository;
        _logger=logger;
    }
    
    public async Task<PortfolioResponseDTO> CreatePortfolioAsync(CreatePortfolioDTO createPortfolioDTO)
    {
        var newPortfolio = _mapper.Map<Portfolio>(createPortfolioDTO);
        await _portfolioRepository.AddAsync(newPortfolio);

        if(newPortfolio is null) 
        {
            _logger.LogWarning("portfolio is not created for the user {UserId}",createPortfolioDTO.UserId);
            throw new KeyNotFoundException("Portfolio is not created");
        }

        _logger.LogInformation("a new portfolio is created for the user {UserId} and the id of the portfolio is {Id}",createPortfolioDTO.UserId,newPortfolio.Id);

        return  _mapper.Map<PortfolioResponseDTO>(newPortfolio);

       
    }

    public async Task DeletePortfolioAsync(Guid id)
    {
         var deletedPortfolio = await _portfolioRepository.DeleteAsync(id);
         if(deletedPortfolio is null)
        {
          _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id}",id);
          throw new KeyNotFoundException("Portfolio not found");
        }
        _logger.LogInformation("a portfolio is deleted for the user {UserId} and the id of the portfolio is {Id}",deletedPortfolio.UserId, deletedPortfolio.Id);
       

    }

    public async Task<PortfolioResponseDTO> GetByIdAsync(Guid id)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(id);
        if(portfolio is null)
        {   
            _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id}",id);
            throw new KeyNotFoundException("Portfolio not found");
        }
        return _mapper.Map<PortfolioResponseDTO>(portfolio);
    }

    public async Task<IEnumerable<PortfolioResponseDTO>> GetPortfoliosByUserAsync(Guid userId)
    {
        var userPortfolios = await _portfolioRepository.Find(x => x.UserId == userId);
        if(userPortfolios is null)
        {
            _logger.LogWarning("no portfolio is found for the user {UserId}",userId);
            throw new KeyNotFoundException("No portfolio is found for the user");
        }
    
        return _mapper.Map<IEnumerable<PortfolioResponseDTO>>(userPortfolios);

    }

    public async Task<PortfolioResponseDTO> UpdatePortfolioAsync(Guid id, UpdatePortfolioDTO updatePortfolioDTO)
    {
       var portfolio = await _portfolioRepository.GetByIdAsync(id);
       if(portfolio is null)
        {
            _logger.LogWarning("a portfolio is not found. The id of the portfolio is {Id}",updatePortfolioDTO.Id);
            throw new KeyNotFoundException("No portfolio is found");
            
        }
            _mapper.Map(updatePortfolioDTO,portfolio);
         await _portfolioRepository.UpdateAsync(portfolio); 

        _logger.LogInformation("a portfolio is updated for the user {UserId} and the id of the portfolio is {Id}",portfolio.UserId, portfolio.Id);

         return _mapper.Map<PortfolioResponseDTO>(portfolio);
        
    }
}
