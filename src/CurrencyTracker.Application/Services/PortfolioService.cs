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
            return null!;
        }
        return _mapper.Map<PortfolioResponseDTO>(portfolio);
    }

    public async Task<IEnumerable<PortfolioResponseDTO>> GetPortfoliosByUserAsync(Guid userId)
    {
        var allPortfolios =  _portfolioRepository.GetAll();
        if(allPortfolios is null)
        {
            return null!;
        }
        var userPortfolios = allPortfolios.Where(p => p.UserId == userId);
        if(userPortfolios is null)
        {
            return null!;
        }
        var mappedPortfolios = _mapper.Map<IEnumerable<PortfolioResponseDTO>>(userPortfolios);

   
        return await Task.FromResult(mappedPortfolios);
     

    }

    public async Task UpdatePortfolioAsync(Guid id, UpdatePortfolioDTO updatePortfolioDTO)
    {
       var portfolio = await _portfolioRepository.GetByIdAsync(id);
       if(portfolio is null)
        {
            return ;
        }
        _mapper.Map(updatePortfolioDTO,portfolio);

         _portfolioRepository.Update(portfolio);

        await _portfolioRepository.SaveAsync();
    }
}
