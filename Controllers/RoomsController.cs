using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;    
using Hotel.Data;
using Hotel.Models;


namespace Hotel.Controllers
{
    public class RoomsController : Controller
    {
        private readonly HotelContext _context;
        public RoomsController(HotelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var room = await _context.Rooms.ToListAsync();
            return View(room);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Number, Capacity, IsOccupied")] Room room)
        {
             if(ModelState.IsValid)
             {
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
             }
             return View(room);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(x=>x.Id== id);
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Number, Capacity, IsOccupied")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Rooms");
        }

        [HttpGet("/api/rooms/bookeddates/{roomId:int}")]
        public async Task<IActionResult> GetBookedDates(int roomId)
        {
            var spans = await _context.Bookings
                .Where(b => b.RoomId == roomId)
                .Select(b => new { b.StartDate, b.EndDate })
                .ToListAsync();

            var days = new List<string>();
            foreach (var s in spans)
            {
                for (var d = s.StartDate.Date; d <= s.EndDate.Date; d = d.AddDays(1))
                    days.Add(d.ToString("yyyy-MM-dd"));
            }

            return Ok(days.Distinct());
        }

        public IActionResult Calendar(int id)
        {
            return View(model: id);
        }
    }
}
