using Microsoft.AspNetCore.Identity;

namespace Hotel.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Booking> Bookings { get; } = new List<Booking>();
    }
}
