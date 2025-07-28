using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Models;


namespace Hotel.Controllers
{
    public class UsersController : Controller
    {
        private readonly HotelContext _context;
        public UsersController(HotelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _context.Users.ToListAsync();
            return View(user);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Email, Password, Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Email, Password, Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Users");
        }
    }
}
