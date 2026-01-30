using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Infrastructure.Migrations;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService=authService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO createUserDTO)
        {
            try
            {
                var result = await _authService.RegisterAsync(createUserDTO);
                return StatusCode(201,result);
            }
            catch(Exception ex)
            {
                return BadRequest(new {message =ex.Message});
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            try
            {
                var result = await _authService.LoginAsync(loginUserDTO);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Unauthorized(new{message=ex.Message});
            }
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] string RefreshToken)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(RefreshToken);
                return Ok(result)

            }
            catch(Exception ex)
            {
                return Unauthorized(new{message=ex.Message});
            }
        }



    }
}
