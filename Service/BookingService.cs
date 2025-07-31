using Hotel.Data;
using Hotel.DTO;
using Hotel.Models;
using Hotel.Models.ViewModels;
using Hotel.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;

namespace Hotel.Service
{
    public class BookingService : IBookingService
    {
        private readonly HotelContext _context;

        public BookingService(HotelContext context)
        {
            _context = context;
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate, int? excludeId = null)
        {
            bool overlap = await _context.Bookings.AnyAsync(b =>
                b.RoomId == roomId &&
                (excludeId == null || b.Id != excludeId) &&
                startDate < b.EndDate &&
                endDate > b.StartDate);

            return !overlap;
        }

        public async Task<BookingResult> CreateAsync(BookingDto dto)
        {
            int result = DateTime.Compare(dto.StartDate, dto.EndDate);
            if (result >= 0)
                return new BookingResult(false, BookingError.InvalidDates, "End date cannot precede start date.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == dto.UserName);
            if (user == null)
                return new BookingResult(false, BookingError.UserNotFound, "No user with this name was found.");

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Number == dto.RoomNumber);
            if( room == null)
                return new BookingResult(false, BookingError.RoomNotFound, "No room with this number was found.");

            if (dto.NoOfPeople > room.Capacity)
                return new BookingResult(false, BookingError.CapacityExceeded, "This room can't accomodate this many people.");

            bool available = await IsRoomAvailableAsync(room.Id, dto.StartDate, dto.EndDate, -1);
            if (!available)
                return new BookingResult(false, BookingError.RoomUnavailable, "This room is not available between these dates."); ;

            var booking = new Booking
            {
                UserId = user.Id,
                RoomId = room.Id,
                NoOfPeople = dto.NoOfPeople,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return new BookingResult(true, BookingError.None, null, booking); ;
        }

        public async Task<BookingResult> UpdateAsync(int id, BookingDto dto)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return new BookingResult(false, BookingError.BookingNotFound, "Booking not found.");

            int result = DateTime.Compare(dto.StartDate, dto.EndDate);
            if (result >= 0)
                return new BookingResult(false, BookingError.InvalidDates, "End date cannot precede start date.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == dto.UserName);
            if (user == null)
                return new BookingResult(false, BookingError.UserNotFound, "No user with this name was found.");

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Number == dto.RoomNumber);
            if (room == null)
                return new BookingResult(false, BookingError.RoomNotFound, "No room with this number was found.");

            if (dto.NoOfPeople > room.Capacity)
                return new BookingResult(false, BookingError.CapacityExceeded, "This room can't accomodate this many people.");

            bool available = await IsRoomAvailableAsync(room.Id, dto.StartDate, dto.EndDate, id);
            if (!available)
                return new BookingResult(false, BookingError.RoomUnavailable, "This room is not available between these dates."); ;


            booking.UserId = user.Id;
            booking.RoomId = room.Id;
            booking.NoOfPeople = dto.NoOfPeople;
            booking.StartDate = dto.StartDate;
            booking.EndDate = dto.EndDate;
            booking.User = user;
            booking.Room = room;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return new BookingResult(true, BookingError.None, null, booking);
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
