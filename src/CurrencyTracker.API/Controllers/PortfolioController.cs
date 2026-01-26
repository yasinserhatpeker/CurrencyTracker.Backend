using CurrencyTracker.Application.DTOs.Portfolios;
using CurrencyTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePortfolioDTO createPortfolioDTO)
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            try
            {
                await _portfolioService.CreatePortfolioAsync(createPortfolioDTO);
                return Ok(new{message = "Portfolio created succesfully!"});
            }
            catch(Exception ex)
            {
                return BadRequest (new{message=ex.Message});
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _portfolioService.GetByIdAsync(id);
                return Ok(user);
            }
        
          catch(Exception ex)
            {
                return NotFound (new{message=ex.Message});
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPortfoliosByUser(Guid userId)
        {
            try
            {
               var portfolios = await _portfolioService.GetPortfoliosByUserAsync(userId);
               return Ok(portfolios);
            }
            catch(Exception ex)
            {
                return NotFound (new{message=ex.Message});
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _portfolioService.RemovePortfolioAsync(id);
                return NoContent();
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePortfolioDTO updatePortfolioDTO)
        {
            if(updatePortfolioDTO.Id != id)
            {
                return BadRequest("ID mismatch");
            }
            try
            {
                await _portfolioService.UpdatePortfolioAsync(id,updatePortfolioDTO);
                return Ok(new{message="Portfolio updated succesfully!"});
            }
            catch(Exception ex)
            {
                return NotFound(new{message=ex.Message});
            }
        }

    }
}
