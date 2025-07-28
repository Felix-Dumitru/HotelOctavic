using System.ComponentModel.DataAnnotations;

namespace Hotel.Models.ViewModels
{
    public class GuestBookingVm
    {
        public int Id { get; set; }

        [Required]
        public int NoOfPeople { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string? RoomNumber { get; set; }
    }
}
