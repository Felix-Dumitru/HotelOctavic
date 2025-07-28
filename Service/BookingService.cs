using Hotel.Data;
using Hotel.Models;
using Hotel.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Service
{
    public class BookingService : IBookingService
    {
        private readonly HotelContext _context;

        public BookingService(HotelContext context)
        {
            _context = context;
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate,
            int? excludeId = null)
        {
            bool overlap = await _context.Bookings.AnyAsync(b =>
                b.RoomId == roomId &&
                (excludeId == null || b.Id != excludeId) &&
                startDate < b.EndDate &&
                endDate > b.StartDate);

            return !overlap;
        }

        public async Task<Booking?> CreateAsync(BookingVm vm)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == vm.UserName);
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Number == vm.RoomNumber);
            if (user == null || room == null)
                return null;

            if (vm.NoOfPeople > room.Capacity)
                return null;

            bool available = await IsRoomAvailableAsync(room.Id, vm.StartDate, vm.EndDate, -1);
            if (!available)
                return null;

            int result = DateTime.Compare(vm.StartDate, vm.EndDate);
            if (result >= 0)
                return null;

            var booking = new Booking
            {
                UserId = user.Id,
                RoomId = room.Id,
                NoOfPeople = vm.NoOfPeople,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<bool?> UpdateAsync(int id, BookingVm vm)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return false;

            bool available = await IsRoomAvailableAsync(booking.RoomId, vm.StartDate, vm.EndDate, id);
            if (!available)
                return false;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == vm.UserName);
            
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Number == vm.RoomNumber);

            if (vm.NoOfPeople > room.Capacity)
                return false;

            if (user == null || room == null)
                return false;

            int result = DateTime.Compare(vm.StartDate, vm.EndDate);
            if (result >= 0)
                return false;

            booking.UserId = user.Id;
            booking.RoomId = room.Id;
            booking.NoOfPeople = vm.NoOfPeople;
            booking.StartDate = vm.StartDate;
            booking.EndDate = vm.EndDate;
            booking.User = user;
            booking.Room = room;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BookingVm?> GetVmAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
                return null;
            return new BookingVm
            {
                Id = booking.Id,
                UserName = booking.User.Name,
                RoomNumber = booking.Room.Number,
                NoOfPeople = booking.NoOfPeople,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate
            };
        }

        public async Task<List<BookingVm>> GetAllVmsAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Select(b => new BookingVm
                {
                    Id = b.Id,
                    UserName = b.User.Name,
                    RoomNumber = b.Room.Number,
                    NoOfPeople = b.NoOfPeople,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate
                })
                .ToListAsync();
        }
    }
}
