using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookings.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "DefaultName";
        public string Phone { get; set; } = "00000000000";
        public string Email { get; set; } = "default@email.com";
        public List<Booking> Bookings { get; set; }
        public List<Blog> Blogs { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        [Required]
        public string Role { get; set; }
    }
}