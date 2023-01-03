using Bookings.Dtos;
using Bookings.Models;

namespace Bookings.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<List<GetUserDto>>> DeleteUser(int id);
        Task<ServiceResponse<List<GetUserDto>>> DeleteCurrentUser(int id);
        Task<ServiceResponse<List<GetUserDto>>> GetAllUsers();
        Task<ServiceResponse<GetUserDto>> GetUserById(int id);
        Task<ServiceResponse<GetUserDto>> GetCurrentUserById(int id);
        Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updatedUser);
        Task<ServiceResponse<GetUserDto>> UpdateCurrentUser(UpdateUserDto updatedUser);
    }
}