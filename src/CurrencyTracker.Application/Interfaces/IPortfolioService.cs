using CurrencyTracker.Application.DTOs.Portfolios;


namespace CurrencyTracker.Application.Interfaces;

public interface IPortfolioService
{    
    Task CreatePortfolioAsync(CreatePortfolioDTO createPortfolioDTO);
    Task<PortfolioResponseDTO> GetByIdAsync(Guid id);
    Task<IEnumerable<PortfolioResponseDTO>> GetPortfoliosByUserAsync(Guid UserId);
    Task UpdatePortfolioAsync(Guid id, UpdatePortfolioDTO updatePortfolioDTO);
    Task DeletePortfolioAsync(Guid id);

}
