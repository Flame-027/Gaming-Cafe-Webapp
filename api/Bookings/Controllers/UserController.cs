using Bookings.Dtos;
using Bookings.Models;
using Bookings.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> Get()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserById {id} - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUserById(int id)
        {
            var response = await _userService.GetUserById(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateUser(UpdateUserDto updatedUser)
        {
            var response = await _userService.UpdateUser(updatedUser);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser {id} - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> DeleteUser(int id)
        {
            var response = await _userService.DeleteUser(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet("GetCurrentUser {id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetCurrentUserById(int id)
        {
            var response = await _userService.GetCurrentUserById(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateCurrentUser(UpdateUserDto updatedUser)
        {
            var response = await _userService.UpdateCurrentUser(updatedUser);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpDelete("DeleteCurrentUser {id}")]
        public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> DeleteCurrentUser(int id)
        {
            var response = await _userService.DeleteCurrentUser(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }
    }
}