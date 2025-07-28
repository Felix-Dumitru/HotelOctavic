namespace Hotel.Models.ViewModels
{
    public class BookingVm
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string RoomNumber { get; set; }
        public int NoOfPeople { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
