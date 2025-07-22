using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hotel.Controllers
{
    public class BookingsController : Controller
    {
        private readonly HotelContext _context;

        public BookingsController(HotelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings.ToListAsync();
            return View(bookings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, UserId, RoomId, StartDate, EndDate")] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return View(booking);
            }

            bool overlap = await _context.Bookings.AnyAsync(b =>
                b.RoomId == booking.RoomId &&
                booking.StartDate < b.EndDate &&
                booking.EndDate > b.StartDate);

            if (overlap)
            {
                ModelState.AddModelError("", "That room is already booked for this period");
                return View(booking);
            }


            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, UserId, RoomId, StartDate, EndDate")] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return View(booking);
            }

            bool overlap = await _context.Bookings.AnyAsync(b =>
                b.RoomId == booking.RoomId &&
                b.Id != booking.Id &&
                booking.StartDate < b.EndDate &&
                booking.EndDate > b.StartDate);

            if (overlap)
            {
                ModelState.AddModelError("", "That room is already booked for this period");
                return View(booking);
            }

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}