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

        await _portfolioRepository.SaveAsync();
    }

    public async Task RemovePortfolioAsync(Guid id)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(id);
        if(portfolio is null)
        {
            throw new Exception("Portfolio is not found");
        }
        _portfolioRepository.Remove(portfolio);

        await _portfolioRepository.SaveAsync();

    }

    public async Task<PortfolioResponseDTO> GetByIdAsync(Guid id)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(id);
        if(portfolio is null)
        {
            throw new Exception("Portfolio is not found");
        }
        return _mapper.Map<PortfolioResponseDTO>(portfolio);
    }

    public async Task<IEnumerable<PortfolioResponseDTO>> GetPortfoliosByUserAsync(Guid userId)
    {
        var userPortfolios = await _portfolioRepository.Find(x => x.UserId == userId);
       if (!userPortfolios.Any()) 
    {
        throw new Exception("No portfolio is found");
    }
        
        return _mapper.Map<IEnumerable<PortfolioResponseDTO>>(userPortfolios);

    }

    public async Task UpdatePortfolioAsync(Guid id, UpdatePortfolioDTO updatePortfolioDTO)
    {
       var portfolio = await _portfolioRepository.GetByIdAsync(id);
       if(portfolio is null)
        {
            throw new Exception("No portfolio is found");
        }
        _mapper.Map(updatePortfolioDTO,portfolio);

         _portfolioRepository.Update(portfolio);

        await _portfolioRepository.SaveAsync();
    }
}
