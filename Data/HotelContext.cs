using Microsoft.EntityFrameworkCore;

using Hotel.Models;

namespace Hotel.Data
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options)
        {

        }

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Room> Rooms { get; set; }
        public DbSet<Models.Booking> Bookings { get; set; }
    }
}