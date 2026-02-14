using CurrencyTracker.Application.DTOs.Portfolios;
using CurrencyTracker.Application.Interfaces;
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
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            try
            {   
                createPortfolioDTO.UserId=GetCurrentUserId();
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
                var portfolio = await _portfolioService.GetByIdAsync(id);
                if(portfolio.UserId != GetCurrentUserId())
                {
                    return Unauthorized(new{message="You have no right to see this portfolio"});
                }
                return Ok(portfolio);

                
            }
        
          catch(Exception ex)
            {
                return NotFound (new{message=ex.Message});
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMyPortfolios()
        {
            try
            {
               var userId = GetCurrentUserId();
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
                var portfolio = await _portfolioService.GetByIdAsync(id);
                if(portfolio.UserId != GetCurrentUserId())
                {
                    return Unauthorized(new{message ="You have no right to delete this portfolio"});
                }
                await _portfolioService.DeletePortfolioAsync(id);
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
                var portfolio = await _portfolioService.GetByIdAsync(id);
                if(portfolio.UserId != GetCurrentUserId())
                {
                    return Unauthorized(new{message="You have no right to update this portfolio"});
                }
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
