using CurrencyTracker.Application.DTOs.Portfolios;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosController : CustomBaseController
    {
        private readonly IPortfolioService _portfolioService;
        public PortfoliosController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;

        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePortfolioDTO createPortfolioDTO)
        {

            createPortfolioDTO.UserId = GetCurrentUserId();
            var portfolio = await _portfolioService.CreatePortfolioAsync(createPortfolioDTO);

            return CreatedAtAction(nameof(GetById),
            new { id = portfolio.Id },
            ApiResponse<PortfolioResponseDTO>.Success(portfolio, "You created the portfolio successfully."));


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id,Guid userId)
        {
        
            var portfolio = await _portfolioService.GetByIdAsync(id,userId);
            if (portfolio.UserId != GetCurrentUserId())
            {
                return Unauthorized(ApiResponse<object>.Fail("You have no right to see this portfolio"));
            }
            return Ok(ApiResponse<PortfolioResponseDTO>.Success(portfolio, "You retrieved the portfolio successfully."));

        }

        [HttpGet]
        public async Task<IActionResult> GetMyPortfolios()
        {

            var userId = GetCurrentUserId();
            var portfolios = await _portfolioService.GetPortfoliosByUserAsync(userId);
            return Ok(ApiResponse<IEnumerable<PortfolioResponseDTO>>.Success(portfolios, "You retrieved the portfolios successfully."));

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        { 
          var currentUserId = GetCurrentUserId();
          await _portfolioService.DeletePortfolioAsync(id, currentUserId);
          return NoContent();
        
        }

        [HttpGet("{id}/summary")]
        public async Task<IActionResult> GetPorfolioSummary(Guid id)
        {  
           var currentUserId = GetCurrentUserId();
           var summary = await _portfolioService.GetPortfolioSummaryAsync(id, currentUserId);
           return Ok(ApiResponse<PortfolioSummaryDTO>.Success(summary, "You retrieved the portfolio summary successfully."));

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePortfolioDTO updatePortfolioDTO)
        {
           var currentUserId = GetCurrentUserId();
           var updatedPortfolio = await _portfolioService.UpdatePortfolioAsync(id, updatePortfolioDTO, currentUserId);
           return Ok(ApiResponse<PortfolioResponseDTO>.Success(updatedPortfolio, "You updated the portfolio successfully."));

        }

    }
}
