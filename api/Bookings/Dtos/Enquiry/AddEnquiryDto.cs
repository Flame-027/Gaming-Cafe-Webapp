namespace Bookings.Dtos
{
    public class AddEnquiryDto
    {
        public string Name { get; set; } = "Name";
        public string Email { get; set; } = "default@email.com";
        public string EnquiryText { get; set; } = "Enquiry Text";
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public bool TicketResolved { get; set; } = false;
    }
}