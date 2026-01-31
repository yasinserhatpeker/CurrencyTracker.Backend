using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CustomBaseController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService=userService;
        }
    
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {   var userId = GetCurrentUserId();
                var user = await _userService.GetByIdAsync(userId);
                return Ok(user);
            }
            catch(Exception ex)
            {
                return NotFound(new{message=ex.Message});
            }
        }

       [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserDTO updateUserDTO)
        {
           
           try
            {    
                var userId = GetCurrentUserId();
                updateUserDTO.Id=userId;
                 await _userService.UpdateUserAsync(userId,updateUserDTO);
                 return Ok(new {message = "User updated succesfully!"});
            } 
            catch(Exception ex)
            {
                return NotFound(new {message = ex.Message});
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            try
            {   
                var userId=GetCurrentUserId();
                await _userService.DeleteUserAsync(userId);
                return NoContent();
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
