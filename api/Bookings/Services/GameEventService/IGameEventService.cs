using Bookings.Dtos;
using Bookings.Models;

namespace Bookings.Services.GameEventService
{
    public interface IGameEventService
    {
        Task<ServiceResponse<List<GetGameEventDto>>> AddGameEvent(AddGameEventDto newGameEvent);
        Task<ServiceResponse<List<GetGameEventDto>>> DeleteGameEvent(int id);
        Task<ServiceResponse<List<GetGameEventDto>>> GetAllGameEvents();
        Task<ServiceResponse<GetGameEventDto>> GetGameEventById(int id);
        Task<ServiceResponse<GetGameEventDto>> UpdateGameEvent(UpdateGameEventDto updatedGameEvent);
    }
}