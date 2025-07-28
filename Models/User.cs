namespace Hotel.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }

        public List<Booking> Bookings { get; } = new List<Booking>();
    }
}