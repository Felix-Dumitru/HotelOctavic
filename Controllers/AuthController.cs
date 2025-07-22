using System.Security.Claims;
using Hotel.Data;
using Hotel.Models;
using Hotel.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class AuthController : Controller
    {
        private readonly HotelContext _context;

        public AuthController(HotelContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (await _context.Users.AnyAsync(u => u.Email == vm.Email))
            {
                ModelState.AddModelError("", "Email already registered");
                return View(vm);
            }

            var user = new User { Email = vm.Email, Password = vm.Password, Role = vm.Role };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginVm {ReturnUrl = returnUrl});
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == vm.Email && u.Password == vm.Password);
            if (existingUser == null)
            {
                ModelState.AddModelError("", "No user exists with this email and password.");
                return View(vm);
            }

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                new Claim(ClaimTypes.Email, existingUser.Email),
                new Claim(ClaimTypes.Role,  existingUser.Role)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

            if (existingUser.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            
            return RedirectToAction("Book", "MyBookings");
        }

        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
