using System;
using CurrencyTracker.Application.DTOs.Portfolios;

namespace CurrencyTracker.Application.Interfaces;

public interface IPortfolioService
{    
    Task CreatePortfolioAsync(CreatePortfolioDTO createPortfolioDTO);
    Task<PortfolioResponseDTO> GetByIdAsync(Guid id);
    Task<IEnumerable<PortfolioResponseDTO>> GetAllPortfoliosAsync();
    Task DeletePortfolioAsync(Guid id);
    Task UpdatePortfolioAsync(UpdatePortfolioDTO updatePortfolioDTO,Guid id);

}
