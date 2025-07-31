using Hotel.DTO;
using Hotel.Models;
using Hotel.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Service.Interfaces
{
    public interface IRoomService
    {
        Task<Room?> CreateAsync(RoomDto dto);
        Task<Room?> UpdateAsync(int id, RoomDto dto);
        Task<bool> DeleteAsync(int id);

        Task<RoomVm?> GetVmAsync(int id);
        Task<List<RoomVm>> GetAllVmsAsync();

        Task<RoomCalendarVm> GetCalendarVmAsync(int id);

        Task<List<string>> GetBookedDatesAsync(int id);
    }
}
