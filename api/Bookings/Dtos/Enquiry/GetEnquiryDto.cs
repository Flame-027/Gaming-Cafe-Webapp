namespace Bookings.Dtos
{
    public class GetEnquiryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Name";
        public string Email { get; set; } = "default@email.com";
        public string EnquiryText { get; set; } = "Enquiry Text";
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public bool TicketResolved { get; set; } = false;
    }
}