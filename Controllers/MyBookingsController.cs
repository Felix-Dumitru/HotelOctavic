using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class MyBookingsController : Controller
    {
        private readonly HotelContext _context;

        public MyBookingsController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet("/Rooms/Book/{roomId:int}")]
        public IActionResult Book(int roomId)
        {
            return View("Book", new Booking { RoomId = roomId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book([Bind("StartDate,EndDate")] Booking booking)
        {
            booking.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!ModelState.IsValid) return View(booking);

            var freeRoom = await _context.Rooms.FirstOrDefaultAsync(r =>
                !_context.Bookings.Any(b =>
                    b.RoomId == r.Id &&
                    booking.StartDate < b.EndDate &&
                    booking.EndDate > b.StartDate));

            if (freeRoom == null)
            {
                ModelState.AddModelError("", "No rooms available for that period.");
                return View(booking);
            }

            booking.RoomId = freeRoom.Id;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyBookings));
        }

        public async Task<IActionResult> MyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myBookings = await _context.Bookings
                .Where(b => b.UserId.ToString() == userId)
                .Include(b => b.Room)
                .ToListAsync();

            return View(myBookings);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserBookings(int id)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == id)
                .Include(b => b.Room)
                .ToListAsync();

            return View(bookings);
        }

        public IActionResult Book()
        {
            return View(new Booking
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            });
        }
    }
}
