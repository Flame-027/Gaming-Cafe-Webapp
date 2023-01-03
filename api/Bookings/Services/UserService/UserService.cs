using System.Security.Claims;
using AutoMapper;
using Bookings.Data;
using Bookings.Dtos;
using Bookings.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<GetUserDto>>> DeleteUser(int id)
        {
            var response = new ServiceResponse<List<GetUserDto>>();
            try
            {
                User user = await _context.Users.FirstAsync(c => c.Id == id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    response.Data = _context.Users.Select(c => _mapper.Map<GetUserDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Character not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> DeleteCurrentUser(int id)
        {
            var response = new ServiceResponse<List<GetUserDto>>();
            try
            {
                User user = await _context.Users.FirstOrDefaultAsync(c => c.Id == id && c.Id == GetUserId());
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    response.Data = _context.Users
                        .Where(c => c.Id == GetUserId())
                        .Select(c => _mapper.Map<GetUserDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "User not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> GetAllUsers()
        {
            var response = new ServiceResponse<List<GetUserDto>>();
            var dbUsers = await _context.Users.ToListAsync();
            response.Data = dbUsers.Select(c => _mapper.Map<GetUserDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            var response = new ServiceResponse<GetUserDto>();
            var dbUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);
            response.Data = _mapper.Map<GetUserDto>(dbUser);
            return response;
        }

        public async Task<ServiceResponse<GetUserDto>> GetCurrentUserById(int id)
        {
            var response = new ServiceResponse<GetUserDto>();
            var dbUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == id && c.Id == GetUserId());
            response.Data = _mapper.Map<GetUserDto>(dbUser);
            return response;
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updatedUser)
        {
            var response = new ServiceResponse<GetUserDto>();
            try
            {
                User user = await _context.Users.FirstOrDefaultAsync(c => c.Id == updatedUser.Id);

                user.Name = updatedUser.Name;
                user.Phone = updatedUser.Phone;
                user.Email = updatedUser.Email;

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetUserDto>(user);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateCurrentUser(UpdateUserDto updatedUser)
        {
            var response = new ServiceResponse<GetUserDto>();
            try
            {
                User user = await _context.Users.FirstOrDefaultAsync(c => c.Id == updatedUser.Id);
                if (user.Id == GetUserId())
                {
                    user.Name = updatedUser.Name;
                    user.Phone = updatedUser.Phone;
                    user.Email = updatedUser.Email;

                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetUserDto>(user);
                }
                else
                {
                    response.Success = false;
                    response.Message = "User not found or user is unauthorised.";
                }
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