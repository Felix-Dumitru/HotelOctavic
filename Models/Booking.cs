using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoomId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int NoOfPeople { get; set; }

        //[ForeignKey("UserId")]
        [ValidateNever] 
        public User User { get; set; }

        //[ForeignKey("RoomId")]
        [ValidateNever] 
        public Room Room { get; set; }
    }
}