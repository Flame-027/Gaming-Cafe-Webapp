namespace Bookings.Dtos.User
{
    public class RegisterUserDto
    {
        public string Name { get; set; } = "DefaultName";
        public string Phone { get; set; } = "00000000000";
        public string Email { get; set; } = "default@email.com";
        public string Username { get; set; }
        public string Password { get; set; }
    }
}