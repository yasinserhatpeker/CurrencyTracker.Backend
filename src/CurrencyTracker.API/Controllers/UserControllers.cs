using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Application.Interfaces;
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
                return BadRequest (new {message = ex.Message});
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
                return NotFound (new{message =ex.Message});
            }

        }
          [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var user = _userService.GetByIdAsync(id);
                return Ok(user);
            }
            catch(Exception ex)
            {
                return NotFound(new{message=ex.Message});
            }
        }

       [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDTO updateUserDTO)
        {
            if(id != updateUserDTO.Id)
            {
                return BadRequest("ID mismatch.");
            }
           try
            {
                 await _userService.UpdateUserAsync(id,updateUserDTO);
                 return Ok(new {message = "User updated succesfully!"});
            } 
            catch(Exception ex)
            {
                return NotFound(new {message = ex.Message});
            }
        }

        

    }
}
