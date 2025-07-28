using System.Security.Claims;
using Hotel.Data;
using Hotel.DTO;
using Hotel.Models;
using Hotel.Models.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Service.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HotelContext _context;

        public AuthService(HotelContext context)
        {
            _context = context;
        }

        public async Task<(bool, string? Error)> RegisterAsync(RegisterVm vm)
        {
            if (await _context.Users.AnyAsync(u => u.Email == vm.Email))
            {
                return (false, "Email already registered");
            }

            var user = new User
            {
                Email = vm.Email,
                Password = vm.Password,
                Role = vm.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool, User? User, string? Error)> ValidateCredentialsAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                return (false, null, "Invalid email or password");
            }

            return (true, user, null);
        }
        public async Task<LoginResult> BuildLoginAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user is null)
                return new LoginResult(false, null, null, "Invalid email or password");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,  user.Role),
            };

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

            string redirect;
            if (user.Role == "Admin")
            {
                redirect = "/Admin/Index";
            }
            else
            {
                redirect = "/MyBookings/Book";
            }


            return new LoginResult(true, principal, redirect, null);
        }

    }

}
