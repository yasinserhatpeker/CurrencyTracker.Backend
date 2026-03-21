using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;


namespace CurrencyTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : CustomBaseController
    {
        private readonly IMarketService _marketService;

        public MarketController(IMarketService marketService)
        {
            _marketService = marketService;
        }
        [HttpGet("price/{baseCurrency}")]
        public async Task<IActionResult> GetLatestData([FromRoute] string baseCurrency, [FromQuery] string quoteCurrency = "TRY")
        {
            if (string.IsNullOrWhiteSpace(baseCurrency))
            {
                return BadRequest(ApiResponse<object>.Fail("BaseCurrency is required"));
            }
            var price = await _marketService.GetMarketPriceAsync(baseCurrency, quoteCurrency);

            return Ok(ApiResponse<MarketPriceDTO>.Success(price, "You retrieved the price successfully."));

        }
    }
}
