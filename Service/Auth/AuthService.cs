using System.Security.Claims;
using Hotel.Data;
using Hotel.Models;
using Hotel.Models.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Hotel.DTO;
using Hotel.DTO.Auth;


namespace Hotel.Service.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HotelContext _context;

        public AuthService(HotelContext context)
        {
            _context = context;
        }

        public async Task<(bool, string? Error)> RegisterAsync(RegisterRequestDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return (false, "Email already registered");
            }

            var user = new User
            {
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role
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

        public async Task<LoginResultDto> BuildLoginAsync(LoginRequestDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);

            if (user is null)
                return new LoginResultDto(false, null, null, "Invalid email or password");

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
                redirect = "/MyBookings/MyBookings";
            }


            return new LoginResultDto(true, principal, redirect, null);
        }

    }

}
