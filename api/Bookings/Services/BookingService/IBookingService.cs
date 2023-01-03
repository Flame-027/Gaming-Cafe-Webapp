using Bookings.Dtos;
using Bookings.Models;

namespace Bookings.Services.BookingService
{
    public interface IBookingService
    {
        Task<ServiceResponse<List<GetBookingDto>>> GetAllBookings();
        Task<ServiceResponse<List<GetBookingDto>>> GetAllCurrentUserBookings();
        Task<ServiceResponse<GetBookingDto>> GetBookingById(int id);
        Task<ServiceResponse<GetBookingDto>> GetUserBookingById(int id);
        Task<ServiceResponse<List<GetBookingDto>>> AddBooking(AddBookingDto newBooking, int id);
        Task<ServiceResponse<List<GetBookingDto>>> AddUserBooking(AddBookingDto newBooking);
        Task<ServiceResponse<GetBookingDto>> UpdateBooking(UpdateBookingDto updatedBooking);
        Task<ServiceResponse<GetBookingDto>> UpdateUserBooking(UpdateBookingDto updatedBooking);
        Task<ServiceResponse<List<GetBookingDto>>> DeleteBooking(int id);
        Task<ServiceResponse<List<GetBookingDto>>> DeleteUserBooking(int id);
        bool IsValidBooking(DateTime start, DateTime end, List<GetBookingDto> allBookings, bool ignoreSelf);
        Task<ServiceResponse<bool>> IsValidBookingSR(DateTime start, DateTime end, bool ignoreSelf);
        int GetPrice(DeskType deskType, int playTime, int groupSize);
    }
}