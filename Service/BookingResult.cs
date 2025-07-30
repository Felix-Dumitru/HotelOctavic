using Hotel.Models;

namespace Hotel.Service
{
    public enum BookingError
    {
        None,
        RoomNotFound,
        UserNotFound,
        RoomUnavailable,
        InvalidDates,
        CapacityExceeded,
        BookingNotFound
    }
    public record BookingResult(bool Success, BookingError Error = BookingError.None, string? Message = null, Booking? Booking = null);
}
