using Hotel.DTO;
using Hotel.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Service.Interfaces
{
    public interface IMyBookingsService
    {
        Task<List<BookingVm>> GetMyBookingsAsync(int id);
        Task<bool> CreateMyBookingAsync(int userId, GuestBookingDto dto);

        Task<GuestBookingVm?> GetVmAsync(int id);
        Task<bool> DeleteMyBookingAsync(int id, int userId);
    }
}
