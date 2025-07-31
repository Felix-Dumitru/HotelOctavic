using Hotel.Data;
using Hotel.DTO;
using Hotel.Models;
using Hotel.Models.ViewModels;
using Hotel.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Service
{
    public class RoomService : IRoomService
    {

        private readonly HotelContext _context;

        public RoomService(HotelContext context)
        {
            _context = context;
        }

        public async Task<Room?> CreateAsync(RoomDto dto)
        {
            var number = dto.Number.Trim();

            if (dto.Capacity <= 0)
                return null;

            var room = new Room
            {
                Number = number,
                Capacity = dto.Capacity
            };

            _context.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Room?> UpdateAsync(int id, RoomDto dto)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return null;

            if (dto.Capacity <= 0)
                return null;

            room.Number = dto.Number.Trim();
            room.Capacity = dto.Capacity;

            _context.Update(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetBookedDatesAsync(int id)
        {
            var spans = await _context.Bookings
                .Where(b => b.RoomId == id)
                .Select(b => new { b.StartDate, b.EndDate })
                .ToListAsync();

            var days = new List<string>();
            foreach (var s in spans)
            {
                for (var d = s.StartDate.Date; d <= s.EndDate.Date; d = d.AddDays(1))
                    days.Add(d.ToString("yyyy-MM-dd"));
            }

            return days;
        }

        public async Task<RoomVm?> GetVmAsync(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);

            if(room == null)
                return null;

            return new RoomVm
            {
                Id = room.Id,
                Number = room.Number,
                Capacity = room.Capacity
            };
        }

        public async Task<RoomCalendarVm> GetCalendarVmAsync(int id)
        {
            var vm = await _context.Rooms
                .Where(r => r.Id == id)
                .Select(r => new RoomCalendarVm
                {
                    RoomId = r.Id,
                    RoomNumber = r.Number
                })
                .SingleOrDefaultAsync();

            return vm;
        }

        public async Task<List<RoomVm>> GetAllVmsAsync()
        {
            return await _context.Rooms
                .Select(r => new RoomVm
                {
                    Id = r.Id,
                    Number = r.Number,
                    Capacity = r.Capacity
                })
                .ToListAsync();
        }
    }
}
