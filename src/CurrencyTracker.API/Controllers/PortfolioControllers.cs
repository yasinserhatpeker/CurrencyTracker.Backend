using CurrencyTracker.Application.DTOs.Portfolios;
using CurrencyTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioControllers : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        public PortfolioControllers(IPortfolioService portfolioService)
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
                return BadRequest(new{message=ex.Message});
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
                return NotFound(new{message=ex.Message});
            }
        }

        
    }
}
