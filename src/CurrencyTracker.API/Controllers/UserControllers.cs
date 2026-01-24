using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService=userService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO createUserDTO)
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            try
            {
                await _userService.CreateUserAsync(createUserDTO);
                return Ok (new {message ="User created succesfully"});
            }
            catch(Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        } 

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
              var users = await _userService.GetAllUsersAsync();
              return Ok(users);

            }
            catch(Exception ex)
            {
                return BadRequest(new{message =ex.Message});
            }
        }
    }
}
