using Hotel.DTO;
using Hotel.Models;
using Hotel.Models.ViewModels;

namespace Hotel.Service.Interfaces
{
    public interface IBookingService
    {
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate, int? bookingId = null);
        Task<BookingResult> CreateAsync(BookingDto dto);
        Task<BookingResult> UpdateAsync(int id, BookingDto dto);

        Task<bool> DeleteAsync(int id);

        Task<BookingVm?> GetVmAsync(int id);
        Task<List<BookingVm>> GetAllVmsAsync();
    }
}
