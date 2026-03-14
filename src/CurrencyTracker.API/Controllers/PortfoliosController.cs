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
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid data"));

            try
            {
                createPortfolioDTO.UserId = GetCurrentUserId();
                var portfolio =  await _portfolioService.CreatePortfolioAsync(createPortfolioDTO);

                return CreatedAtAction(nameof(GetById), 
                new { id = portfolio.Id }, 
                ApiResponse<PortfolioResponseDTO>.Success(portfolio, "You created the portfolio successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var portfolio = await _portfolioService.GetByIdAsync(id);
                if (portfolio.UserId != GetCurrentUserId())
                {
                    return Unauthorized(ApiResponse<object>.Fail("You have no right to see this portfolio"));
                }
                return Ok(ApiResponse<PortfolioResponseDTO>.Success(portfolio, "You retrieved the portfolio successfully."));


            }

            catch (Exception ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMyPortfolios()
        {
            try
            {
                var userId = GetCurrentUserId();
                var portfolios = await _portfolioService.GetPortfoliosByUserAsync(userId);
                return Ok(ApiResponse<IEnumerable<PortfolioResponseDTO>>.Success(portfolios, "You retrieved the portfolios successfully."));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var portfolio = await _portfolioService.GetByIdAsync(id);
                if (portfolio.UserId != GetCurrentUserId())
                {
                    return Unauthorized(ApiResponse<object>.Fail("You have no right to delete this portfolio"));
                }
                await _portfolioService.DeletePortfolioAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePortfolioDTO updatePortfolioDTO)
        {
            if (updatePortfolioDTO.Id != id)
            {
                return BadRequest(ApiResponse<object>.Fail("ID mismatch"));
            }
            try
            {
                var portfolio = await _portfolioService.GetByIdAsync(id);
                if (portfolio.UserId != GetCurrentUserId())
                {
                    return Unauthorized(ApiResponse<object>.Fail("You have no right to update this portfolio"));
                }
               var updatedPortfolio = await _portfolioService.UpdatePortfolioAsync(id, updatePortfolioDTO);
                return Ok(ApiResponse<PortfolioResponseDTO>.Success(updatedPortfolio,"You updated the portfolio successfully."));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
        }

    }
}
