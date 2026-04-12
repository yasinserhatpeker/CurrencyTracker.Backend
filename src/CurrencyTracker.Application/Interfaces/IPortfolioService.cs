using CurrencyTracker.Application.DTOs.Portfolios;


namespace CurrencyTracker.Application.Interfaces;

public interface IPortfolioService
{    
    Task<PortfolioResponseDTO> CreatePortfolioAsync(CreatePortfolioDTO createPortfolioDTO);
    Task<PortfolioResponseDTO> GetByIdAsync(Guid id,Guid userId);
    Task<IEnumerable<PortfolioResponseDTO>> GetPortfoliosByUserAsync(Guid userId);
    Task<PortfolioResponseDTO> UpdatePortfolioAsync(Guid id, UpdatePortfolioDTO updatePortfolioDTO,Guid userId);
    Task DeletePortfolioAsync(Guid id,Guid userId);
    Task<PortfolioSummaryDTO> GetPortfolioSummaryAsync(Guid id,Guid userId);

}