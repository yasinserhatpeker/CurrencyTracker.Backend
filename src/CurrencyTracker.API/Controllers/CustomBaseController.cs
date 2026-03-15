using System.Security.Claims;
using CurrencyTracker.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.Controllers
{
    
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
            throw new UnauthorizedAccessException("You are not authorized");
        }
    }
}
