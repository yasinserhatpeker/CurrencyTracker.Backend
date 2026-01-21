using System;
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
        
    }

    public Task DeletePortfolioAsync(Guid id)
    {
        throw new NotImplementedException();
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
