using System.Security.Claims;
using AutoMapper;
using Bookings.Data;
using Bookings.Dtos;
using Bookings.Models;
using Itenso.TimePeriod;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Services.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private static int _openingHourWeekDay = new DateTime(1, 1, 1, 15, 0, 0).Hour;
        private static int _closingHourWeekDay = new DateTime(1, 1, 1, 22, 0, 0).Hour;
        private static int _openingHourWeekEnd = new DateTime(1, 1, 1, 10, 0, 0).Hour;
        private static int _closingHourWeekEnd = new DateTime(1, 1, 1, 22, 0, 0).Hour;
        private static int _anyDeskPrice = 5;
        private static int _groupDeskPrice = 8;
        private static int _singleRoomPrice = 90;
        private static int _doubledRoomPrice = 120;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = context;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<GetBookingDto>>> AddBooking(AddBookingDto newBooking, int id)
        {
            var response = new ServiceResponse<List<GetBookingDto>>();
            Booking booking = _mapper.Map<Booking>(newBooking);
            // Assign found id of User to Booking.UserId
            var dbUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);
            if (dbUser != null)
            {
                booking.UserId = dbUser.Id;
            }
            else
            {
                response.Success = false;
                response.Message = "Unable to find User with that Id.";
                return response;
            }
            // List of bookings for IsValidBooking
            var dbBookings = await _context.Bookings.ToListAsync();
            var allBookings = dbBookings.Select(c => _mapper.Map<GetBookingDto>(c)).ToList();
            if (!IsValidBooking(newBooking.BookingTime, newBooking.BookingTime.AddHours(newBooking.PlayTime), allBookings, false))
            {
                response.Success = false;
                response.Message = "Invalid booking data provided - check the console for more info.";
                return response;
            }

            booking.Price = GetPrice(newBooking.DeskType, newBooking.PlayTime, newBooking.GroupSize);
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            response.Data = await _context.Bookings.Select(c => _mapper.Map<GetBookingDto>(c)).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<List<GetBookingDto>>> AddUserBooking(AddBookingDto newBooking)
        {
            var response = new ServiceResponse<List<GetBookingDto>>();
            Booking booking = _mapper.Map<Booking>(newBooking);

            booking.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            // List of bookings for IsValidBooking
            var dbBookings = await _context.Bookings.ToListAsync();
            var allBookings = dbBookings.Select(c => _mapper.Map<GetBookingDto>(c)).ToList();
            if (!IsValidBooking(newBooking.BookingTime, newBooking.BookingTime.AddHours(newBooking.PlayTime), allBookings, false))
            {
                response.Success = false;
                response.Message = "Invalid booking data provided - check the console for more info.";
                return response;
            }

            booking.Price = GetPrice(newBooking.DeskType, newBooking.PlayTime, newBooking.GroupSize);
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            response.Data = await _context.Bookings
                .Where(c => c.User.Id == GetUserId())
                .Select(c => _mapper.Map<GetBookingDto>(c)).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<GetBookingDto>> GetBookingById(int id)
        {
            var response = new ServiceResponse<GetBookingDto>();
            var dbBooking = await _context.Bookings.FirstOrDefaultAsync(c => c.Id == id);
            response.Data = _mapper.Map<GetBookingDto>(dbBooking);
            return response;
        }

        public async Task<ServiceResponse<GetBookingDto>> GetUserBookingById(int id)
        {
            var response = new ServiceResponse<GetBookingDto>();
            var dbBooking = await _context.Bookings.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            response.Data = _mapper.Map<GetBookingDto>(dbBooking);
            return response;
        }

        public async Task<ServiceResponse<List<GetBookingDto>>> GetAllBookings()
        {
            var response = new ServiceResponse<List<GetBookingDto>>();
            var dbBookings = await _context.Bookings.ToListAsync();
            response.Data = dbBookings.Select(c => _mapper.Map<GetBookingDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<GetBookingDto>>> GetAllCurrentUserBookings()
        {
            var response = new ServiceResponse<List<GetBookingDto>>();
            var dbBookings = await _context.Bookings.Where(c => c.User.Id == GetUserId()).ToListAsync();
            response.Data = dbBookings.Select(c => _mapper.Map<GetBookingDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetBookingDto>> UpdateBooking(UpdateBookingDto updatedBooking)
        {
            var response = new ServiceResponse<GetBookingDto>();
            try
            {
                Booking booking = await _context.Bookings.FirstOrDefaultAsync(c => c.Id == updatedBooking.Id);

                // List of bookings for IsValidBooking
                var dbBookings = await _context.Bookings.ToListAsync();
                var allBookings = dbBookings.Select(c => _mapper.Map<GetBookingDto>(c)).ToList();

                if (IsValidBooking(updatedBooking.BookingTime, updatedBooking.BookingTime.AddHours(updatedBooking.PlayTime), allBookings, true))
                {
                    booking.DeskType = updatedBooking.DeskType;
                    booking.BookingTime = updatedBooking.BookingTime;
                    booking.PlayTime = updatedBooking.PlayTime;
                    booking.Price = GetPrice(updatedBooking.DeskType, updatedBooking.PlayTime, updatedBooking.GroupSize);
                    booking.GroupSize = updatedBooking.GroupSize;

                    await _context.SaveChangesAsync();
                    response.Data = _mapper.Map<GetBookingDto>(booking);
                }
                else
                {
                    response.Success = false;
                    response.Message = "The booking failed to update as the chosen reservation period has already been booked.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetBookingDto>> UpdateUserBooking(UpdateBookingDto updatedBooking)
        {
            var response = new ServiceResponse<GetBookingDto>();
            try
            {
                Booking booking = await _context.Bookings
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedBooking.Id);
                if (booking.User.Id == GetUserId())
                {

                    // List of bookings for IsValidBooking
                    var dbBookings = await _context.Bookings.ToListAsync();
                    var allBookings = dbBookings.Select(c => _mapper.Map<GetBookingDto>(c)).ToList();

                    if (IsValidBooking(updatedBooking.BookingTime, updatedBooking.BookingTime.AddHours(updatedBooking.PlayTime), allBookings, true))
                    {
                        booking.DeskType = updatedBooking.DeskType;
                        booking.BookingTime = updatedBooking.BookingTime;
                        booking.PlayTime = updatedBooking.PlayTime;
                        booking.Price = GetPrice(updatedBooking.DeskType, updatedBooking.PlayTime, updatedBooking.GroupSize);
                        booking.GroupSize = updatedBooking.GroupSize;

                        await _context.SaveChangesAsync();
                        response.Data = _mapper.Map<GetBookingDto>(booking);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "The booking failed to update as the chosen reservation period has already been booked, or the new booking is invalid.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Booking not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public int GetPrice(DeskType deskType, int playTime, int groupSize)
        {
            if (deskType.ToString() == "AnyDesk")
            {
                return playTime * _anyDeskPrice * groupSize;
            }
            else if (deskType.ToString() == "GroupDesk")
            {
                return playTime * _groupDeskPrice * groupSize;
            }
            else if (deskType.ToString() == "SingleRoom")
            {
                return playTime * _singleRoomPrice * ((groupSize + 9) / 10); // Multiplier is rounded down to the nearest multiple of 10, to a minimum of 1
            }
            else if (deskType.ToString() == "DoubledRoom")
            {
                return playTime * _doubledRoomPrice * ((groupSize + 9) / 10); // Multiplier is rounded down to the nearest multiple of 10, to a minimum of 1
            }
            else
            {
                return 0;
            }
        }

        public bool IsValidBooking(DateTime start, DateTime end, List<GetBookingDto> allBookings, bool ignoreSelf)
        {
            if (!TimeCompare.IsSameDay(start, end))
            {
                Console.WriteLine("Invalid: Multiple day reservation.");
                return false;
            }
            if (start == end)
            {
                System.Console.WriteLine("Invalid: Reservation has no PlayTime length!");
                return false;
            }

            var inputTimeRange = new TimeRange(start, end);

            TimeRange validDays = new TimeRange(DateTime.Now.AddDays(1), DateTime.Now.AddMonths(1));
            if (!validDays.HasInside(inputTimeRange))
            {
                Console.WriteLine("Invalid: Chosen reservation day cannot be within 24 hours and cannot be over a month in advance.");
                return false;
            }

            // Check all other reservations for overlap
            foreach (var bookingSlot in allBookings)
            {
                TimeRange bookedSlot = new TimeRange(bookingSlot.BookingTime, bookingSlot.BookingTime.AddHours(bookingSlot.PlayTime));
                if (inputTimeRange.OverlapsWith(bookedSlot))
                {
                    if (ignoreSelf)
                    {
                        ignoreSelf = false;
                        continue;
                    }
                    Console.WriteLine("Chosen reservation period has already been booked.");
                    return false;
                }
            }

            if (start.DayOfWeek == DayOfWeek.Saturday || start.DayOfWeek == DayOfWeek.Sunday)
            {
                Console.WriteLine("The given day is a Weekend");
                TimeRange workingHours = new TimeRange(TimeTrim.Hour(start, _openingHourWeekEnd), TimeTrim.Hour(start, _closingHourWeekEnd));
                if (!workingHours.HasInside(inputTimeRange))
                {
                    Console.WriteLine("Reservation is not within opening hours!");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("The given day is a Weekday");
                TimeRange workingHours = new TimeRange(TimeTrim.Hour(start, _openingHourWeekDay), TimeTrim.Hour(start, _closingHourWeekDay));
                if (!workingHours.HasInside(inputTimeRange))
                {
                    Console.WriteLine("Reservation is not within opening hours!");
                    return false;
                }
            }
            return true;
        }

        // Repeat IsValidBooking() method with ServiceResponse
        public async Task<ServiceResponse<bool>> IsValidBookingSR(DateTime start, DateTime end, bool ignoreSelf)
        {
            var response = new ServiceResponse<bool>();

            if (!TimeCompare.IsSameDay(start, end))
            {
                response.Success = false;
                response.Message = "Invalid: Multiple day reservation.";
                return response;
            }
            if (start == end)
            {
                response.Success = false;
                response.Message = "Invalid: Reservation has no PlayTime length!";
                return response;
            }

            var inputTimeRange = new TimeRange(start, end);

            TimeRange validDays = new TimeRange(DateTime.Now.AddDays(1), DateTime.Now.AddMonths(1));
            if (!validDays.HasInside(inputTimeRange))
            {
                response.Success = false;
                response.Message = "Invalid: Chosen reservation day cannot be within 24 hours and cannot be over a month in advance.";
                return response;
            }

            // Check all other reservations for overlap
            var allBookings = await _context.Bookings.ToListAsync();
            foreach (var bookingSlot in allBookings)
            {
                TimeRange bookedSlot = new TimeRange(bookingSlot.BookingTime, bookingSlot.BookingTime.AddHours(bookingSlot.PlayTime));
                if (inputTimeRange.OverlapsWith(bookedSlot))
                {
                    if (ignoreSelf)
                    {
                        ignoreSelf = false;
                        continue;
                    }
                    response.Success = false;
                    response.Message = "Chosen reservation period has already been booked.";
                    return response;
                }
            }

            if (start.DayOfWeek == DayOfWeek.Saturday || start.DayOfWeek == DayOfWeek.Sunday)
            {
                Console.WriteLine("The given day is a Weekend");
                TimeRange workingHours = new TimeRange(TimeTrim.Hour(start, _openingHourWeekEnd), TimeTrim.Hour(start, _closingHourWeekEnd));
                if (!workingHours.HasInside(inputTimeRange))
                {
                    response.Success = false;
                    response.Message = "Reservation is not within opening hours!";
                    return response;
                }
            }
            else
            {
                Console.WriteLine("The given day is a Weekday");
                TimeRange workingHours = new TimeRange(TimeTrim.Hour(start, _openingHourWeekDay), TimeTrim.Hour(start, _closingHourWeekDay));
                if (!workingHours.HasInside(inputTimeRange))
                {
                    response.Success = false;
                    response.Message = "Reservation is not within opening hours!";
                    return response;
                }
            }
            response.Data = true;
            return response;
        }

        public async Task<ServiceResponse<List<GetBookingDto>>> DeleteBooking(int id)
        {
            var response = new ServiceResponse<List<GetBookingDto>>();
            try
            {
                Booking booking = await _context.Bookings.FirstAsync(c => c.Id == id);
                if (booking != null)
                {
                    _context.Bookings.Remove(booking);
                    await _context.SaveChangesAsync();
                    response.Data = _context.Bookings.Select(c => _mapper.Map<GetBookingDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Booking not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetBookingDto>>> DeleteUserBooking(int id)
        {
            var response = new ServiceResponse<List<GetBookingDto>>();
            try
            {
                Booking booking = await _context.Bookings.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
                if (booking != null)
                {
                    _context.Bookings.Remove(booking);
                    await _context.SaveChangesAsync();
                    response.Data = _context.Bookings
                        .Where(c => c.User.Id == GetUserId())
                        .Select(c => _mapper.Map<GetBookingDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Booking not found.";
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