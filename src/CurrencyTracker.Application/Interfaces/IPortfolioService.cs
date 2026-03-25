using CurrencyTracker.Application.DTOs.Portfolios;


namespace CurrencyTracker.Application.Interfaces;

public interface IPortfolioService
{    
    Task<PortfolioResponseDTO> CreatePortfolioAsync(CreatePortfolioDTO createPortfolioDTO);
    Task<PortfolioResponseDTO> GetByIdAsync(Guid id);
    Task<IEnumerable<PortfolioResponseDTO>> GetPortfoliosByUserAsync(Guid userId);
    Task<PortfolioResponseDTO> UpdatePortfolioAsync(Guid id, UpdatePortfolioDTO updatePortfolioDTO);
    Task DeletePortfolioAsync(Guid id);
    Task<PortfolioSummaryDTO> GetPortfolioSummaryAsync(Guid id,Guid userId);

}