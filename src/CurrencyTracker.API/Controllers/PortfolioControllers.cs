using CurrencyTracker.Application.Interfaces;
using Microsoft.AspNetCore.Http;
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
        
    }
}
