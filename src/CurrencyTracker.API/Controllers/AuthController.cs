using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Auth;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : CustomBaseController
    {
        private readonly IAuthService _authService;
        private readonly IUserAccountService _userAccountService;
        private readonly ITokenService _tokenService;



        public AuthController(IAuthService authService, IUserAccountService userAccountService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
            _userAccountService = userAccountService;

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO createUserDTO)
        {

            var result = await _authService.RegisterAsync(createUserDTO);
            return StatusCode(201, ApiResponse<UserResponseDTO>.Success(result, "You registered successfully."));

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {

            var result = await _authService.LoginAsync(loginUserDTO);
            return Ok(ApiResponse<object>.Success(result, "You logged in successfully."));

        }

        [HttpPost("refreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshTokenDTO)
        {

            var result = await _tokenService.RefreshTokenAsync(refreshTokenDTO);
            return Ok(ApiResponse<object>.Success(result, "You refreshed token successfully."));

        }

        [HttpPost("google-login")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDTO googleLoginDTO)
        {

            var result = await _authService.GoogleLoginAsync(googleLoginDTO);
            return Ok(ApiResponse<object>.Success(result, "You logged in successfully."));

        }

        [HttpPost("logout")]

        public async Task<IActionResult> Logout([FromBody] RefreshTokenDTO refreshTokenDTO)
        {

            await _tokenService.LogoutAsync(refreshTokenDTO);

            return Ok(ApiResponse<object>.Success(message: "You logged out successfully."));

        }

        [HttpGet("verify-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var IsEmailVerified = await _userAccountService.EmailVerificationAsync(token);
            if (IsEmailVerified)
            {
                return Ok(ApiResponse<object>.Success(message: "Email verification successful."));
            }
            else
            {
                return BadRequest(ApiResponse<object>.Fail("Email verification failed."));

            }


        }
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {

            await _userAccountService.ForgotPasswordAsync(forgotPasswordDTO);
            return Ok(ApiResponse<object>.Success(message: "Password reset link has been sent to your email."));

        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
           
            await _userAccountService.ResetPasswordAsync(resetPasswordDTO);
            return Ok(ApiResponse<object>.Success(message: "Password reset successfully."));

        }
    }
}
