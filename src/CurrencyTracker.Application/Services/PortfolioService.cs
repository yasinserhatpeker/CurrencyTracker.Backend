using AutoMapper;
using CurrencyTracker.Application.DTOs.Portfolios;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entities;
using CurrencyTracker.Domain.Interfaces;

namespace CurrencyTracker.Application.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Portfolio> _portfolioRepository;

    public PortfolioService(IMapper mapper, IGenericRepository<Portfolio> portfolioRepository)
    {
        _mapper=mapper;
        _portfolioRepository= portfolioRepository;
    }
    
    public async Task CreatePortfolioAsync(CreatePortfolioDTO createPortfolioDTO)
    {
        var newPortfolio = _mapper.Map<Portfolio>(createPortfolioDTO);

        await _portfolioRepository.AddAsync(newPortfolio);

       
    }

    public async Task DeletePortfolioAsync(Guid id)
    {
         var deletedPortfolio = await _portfolioRepository.DeleteAsync(id);
         if(deletedPortfolio is null)
        {
          throw new KeyNotFoundException("Portfolio not found.");
        }
       

    }

    public async Task<PortfolioResponseDTO> GetByIdAsync(Guid id)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(id);
        if(portfolio is null)
        {
            throw new KeyNotFoundException("Portfolio is not found");
        }
        return _mapper.Map<PortfolioResponseDTO>(portfolio);
    }

    public async Task<IEnumerable<PortfolioResponseDTO>> GetPortfoliosByUserAsync(Guid userId)
    {
        var userPortfolios = await _portfolioRepository.Find(x => x.UserId == userId);
    
        return _mapper.Map<IEnumerable<PortfolioResponseDTO>>(userPortfolios);

    }

    public async Task UpdatePortfolioAsync(Guid id, UpdatePortfolioDTO updatePortfolioDTO)
    {
       var portfolio = await _portfolioRepository.GetByIdAsync(id);
       if(portfolio is null)
        {
            throw new KeyNotFoundException("No portfolio is found");
        }
         _mapper.Map(updatePortfolioDTO,portfolio);

         await _portfolioRepository.UpdateAsync(portfolio); 


    }
}
