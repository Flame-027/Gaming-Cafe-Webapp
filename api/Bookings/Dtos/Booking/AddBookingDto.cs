using Bookings.Models;

namespace Bookings.Dtos
{
    public class AddBookingDto
    {
        public DeskType DeskType { get; set; }
        public DateTime BookingTime { get; set; }
        public int PlayTime { get; set; }
        public int Price { get; set; }
        public int GroupSize { get; set; }
    }
}