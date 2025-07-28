using Hotel.Models;
using Hotel.Models.ViewModels;

namespace Hotel.Service
{
    public interface IBookingService
    {
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate, int? bookingId = null);
        Task<Booking?> CreateAsync(BookingVm vm);
        Task<bool?> UpdateAsync(int id, BookingVm vm);

        Task<bool> DeleteAsync(int id);

        Task<BookingVm?> GetVmAsync(int id);

        Task<List<BookingVm>> GetAllVmsAsync();
    }
}
