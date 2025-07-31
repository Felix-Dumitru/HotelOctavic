namespace Hotel.Models
{
    public enum Type
    {
        Single,
        Double,
        Triple,
        Quad
    }

    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int Capacity { get; set; }

        //public List<string> Features { get; set; }
        public List<Booking> Bookings { get; } = new List<Booking>();
    }
}
