namespace Bookings.Dtos
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "DefaultName";
        public string Phone { get; set; } = "00000000000";
        public string Email { get; set; } = "default@email.com";
    }
}