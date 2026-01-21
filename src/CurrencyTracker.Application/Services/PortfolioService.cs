using System;
using System.Diagnostics.CodeAnalysis;
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

    public async Task DeletePortfolioAsync(Guid id)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(id);
        if(portfolio is null)
        {
            return;
        }
        _portfolioRepository.Remove(portfolio);
        
        await _portfolioRepository.SaveAsync();

    }

    public Task<PortfolioResponseDTO> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PortfolioResponseDTO>> GetPortfoliosByUserAsync(Guid UserId)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePortfolioAsync(Guid id, UpdatePortfolioDTO updatePortfolioDTO)
    {
        throw new NotImplementedException();
    }
}
