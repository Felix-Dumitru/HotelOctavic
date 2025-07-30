namespace Hotel.DTO
{
    public record BookingDto(
        int Id,         
        string UserName,
        string RoomNumber,
        int NoOfPeople,
        DateTime StartDate,
        DateTime EndDate
    );
}
