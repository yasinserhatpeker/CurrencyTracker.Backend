using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class CustomBaseController : ControllerBase
    {
         protected Guid GetCurrentUserId()
        {
             var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

             if(Guid.TryParse(idClaim, out Guid userId))
            {
                 return userId;
            }
            throw new UnauthorizedAccessException("Token is invalid, user ID is not found");
        }
    }
}
