using Bookings.Dtos;
using Bookings.Models;
using Bookings.Services.GameEventService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameEventController : ControllerBase
    {
        private readonly IGameEventService _gameEventService;
        public GameEventController(IGameEventService gameEventService)
        {
            _gameEventService = gameEventService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddGameEvent - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<List<GetGameEventDto>>>> AddGameEvent(AddGameEventDto newGameEvent)
        {
            return Ok(await _gameEventService.AddGameEvent(newGameEvent));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateGameevent - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<GetGameEventDto>>> UpdateGameEvent(UpdateGameEventDto updatedGameEvent)
        {
            var response = await _gameEventService.UpdateGameEvent(updatedGameEvent);
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
        [HttpDelete("DeleteGameEvent {id} - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<List<GetGameEventDto>>>> DeleteGameEvent(int id)
        {
            var response = await _gameEventService.DeleteGameEvent(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet("GetAllGameEvents")]
        public async Task<ActionResult<ServiceResponse<List<GetGameEventDto>>>> GetAllGameEvents()
        {
            return Ok(await _gameEventService.GetAllGameEvents());
        }

        [HttpGet("GetGameEventById {id}")]
        public async Task<ActionResult<ServiceResponse<GetGameEventDto>>> GetGameEventById(int id)
        {
            var response = await _gameEventService.GetGameEventById(id);
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