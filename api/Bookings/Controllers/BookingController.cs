using Bookings.Dtos;
using Bookings.Models;
using Bookings.Services.BookingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddBooking {id} - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<List<GetBookingDto>>>> AddBooking(AddBookingDto newBooking, int id)
        {
            return Ok(await _bookingService.AddBooking(newBooking, id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<List<GetBookingDto>>>> GetAllBookings()
        {
            return Ok(await _bookingService.GetAllBookings());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetBookingById {id} - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<GetBookingDto>>> GetBookingById(int id)
        {
            var response = await _bookingService.GetBookingById(id);
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
        [HttpPut("UpdateBooking - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<GetBookingDto>>> UpdateBooking(UpdateBookingDto updatedBooking)
        {
            var response = await _bookingService.UpdateBooking(updatedBooking);
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
        [HttpDelete("DeleteBooking {id} - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<List<GetBookingDto>>>> DeleteBooking(int id)
        {
            var response = await _bookingService.DeleteBooking(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost("AddBookingToCurrentUser")]
        public async Task<ActionResult<ServiceResponse<List<GetBookingDto>>>> AddUserBooking(AddBookingDto newBooking)
        {
            return Ok(await _bookingService.AddUserBooking(newBooking));
        }

        [HttpGet("GetAllCurrentUserBookings")]
        public async Task<ActionResult<ServiceResponse<List<GetBookingDto>>>> GetAllCurrentUserBookings()
        {
            return Ok(await _bookingService.GetAllCurrentUserBookings());
        }

        [HttpGet("GetUserBookingById {id}")]
        public async Task<ActionResult<ServiceResponse<GetBookingDto>>> GetUserBookingById(int id)
        {
            var response = await _bookingService.GetUserBookingById(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPut("UpdateUserBooking")]
        public async Task<ActionResult<ServiceResponse<GetBookingDto>>> UpdateUserBooking(UpdateBookingDto updatedBooking)
        {
            var response = await _bookingService.UpdateUserBooking(updatedBooking);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpDelete("DeleteUserBooking {id}")]
        public async Task<ActionResult<ServiceResponse<List<GetBookingDto>>>> DeleteUserBooking(int id)
        {
            var response = await _bookingService.DeleteUserBooking(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetBoolIsValidBooking")]
        public async Task<ActionResult<ServiceResponse<bool>>> IsValidBookingSR(DateTime start, DateTime end, bool ignoreSelf)
        {
            return Ok(await _bookingService.IsValidBookingSR(start, end, false));
        }
    }
}