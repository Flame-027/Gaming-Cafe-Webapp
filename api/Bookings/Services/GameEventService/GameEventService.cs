using AutoMapper;
using Bookings.Data;
using Bookings.Dtos;
using Bookings.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Services.GameEventService
{
    public class GameEventService : IGameEventService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public GameEventService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<ServiceResponse<List<GetGameEventDto>>> AddGameEvent(AddGameEventDto newGameEvent)
        {
            var response = new ServiceResponse<List<GetGameEventDto>>();
            GameEvent gameEvent = _mapper.Map<GameEvent>(newGameEvent);

            _context.GameEvents.Add(gameEvent);
            await _context.SaveChangesAsync();

            var dbGameEvents = await _context.GameEvents.ToListAsync();
            response.Data = dbGameEvents.Select(c => _mapper.Map<GetGameEventDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<GetGameEventDto>>> DeleteGameEvent(int id)
        {
            var response = new ServiceResponse<List<GetGameEventDto>>();
            try
            {
                GameEvent gameEvent = await _context.GameEvents.FirstOrDefaultAsync(c => c.Id == id);
                if (gameEvent != null)
                {
                    _context.GameEvents.Remove(gameEvent);
                    await _context.SaveChangesAsync();
                    response.Data = _context.GameEvents
                        .Select(c => _mapper.Map<GetGameEventDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Event not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetGameEventDto>>> GetAllGameEvents()
        {
            var response = new ServiceResponse<List<GetGameEventDto>>();
            var dbGameEvents = await _context.GameEvents.ToListAsync();
            response.Data = dbGameEvents.Select(c => _mapper.Map<GetGameEventDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetGameEventDto>> GetGameEventById(int id)
        {
            var response = new ServiceResponse<GetGameEventDto>();
            var dbGameEvent = await _context.GameEvents.FirstOrDefaultAsync(c => c.Id == id);
            response.Data = _mapper.Map<GetGameEventDto>(dbGameEvent);
            return response;
        }

        public async Task<ServiceResponse<GetGameEventDto>> UpdateGameEvent(UpdateGameEventDto updatedGameEvent)
        {
            var response = new ServiceResponse<GetGameEventDto>();
            try
            {
                GameEvent gameEvent = await _context.GameEvents.FirstOrDefaultAsync(c => c.Id == updatedGameEvent.Id);

                gameEvent.Title = updatedGameEvent.Title;
                gameEvent.ShortDescription = updatedGameEvent.ShortDescription;
                gameEvent.EventType = updatedGameEvent.EventType;
                gameEvent.Reward = updatedGameEvent.Reward;
                gameEvent.Game = updatedGameEvent.Game;
                gameEvent.Platform = updatedGameEvent.Platform;
                gameEvent.StartTime = updatedGameEvent.StartTime;
                gameEvent.TeamSize = updatedGameEvent.TeamSize;
                gameEvent.BracketType = updatedGameEvent.BracketType;
                gameEvent.RuleSet = updatedGameEvent.RuleSet;

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetGameEventDto>(gameEvent);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}