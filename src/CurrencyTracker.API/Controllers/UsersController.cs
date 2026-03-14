using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Wrappers;
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
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();
            var user = await _userService.GetByIdAsync(userId);
            return Ok(ApiResponse<UserResponseDTO>.Success(user, "You retrieved your profile successfully."));

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserDTO updateUserDTO)
        {
            var userId = GetCurrentUserId();
            updateUserDTO.Id = userId;
            var updatedUser = await _userService.UpdateUserAsync(userId, updateUserDTO);
            return Ok(ApiResponse<UserResponseDTO>.Success(updatedUser, "You updated your profile successfully."));

        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var userId = GetCurrentUserId();
            await _userService.DeleteUserAsync(userId);
            return NoContent();


        }

    }
}
