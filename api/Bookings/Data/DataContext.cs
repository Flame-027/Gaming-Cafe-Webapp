using Bookings.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Enquiry> Enquirys { get; set; }
        public DbSet<GameEvent> GameEvents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(user => user.Role).HasDefaultValue("Customer");
        }
    }
}