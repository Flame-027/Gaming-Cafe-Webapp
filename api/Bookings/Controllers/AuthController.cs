using Bookings.Data;
using Bookings.Dtos.User;
using Bookings.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(RegisterUserDto request)
        {
            var response = await _authRepository.Register(
                new User { Name = request.Name, Phone = request.Phone, Email = request.Email, Username = request.Username }, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Register(LoginUserDto request)
        {
            var response = await _authRepository.Login(
                request.Username, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }
    }
}