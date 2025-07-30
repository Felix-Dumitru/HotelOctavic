using System.ComponentModel.DataAnnotations;

namespace Hotel.DTO
{
    public record GuestBookingDto(int Id, int NoOfPeople, DateTime StartDate, DateTime EndDate, string? RoomNumber);
}
