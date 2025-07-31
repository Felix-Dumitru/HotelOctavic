using Hotel.Data;
using Hotel.DTO;
using Hotel.Models;
using Hotel.Models.ViewModels;
using Hotel.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Service
{
    public class UserService : IUserService
    {
        private readonly HotelContext _context;

        public UserService(HotelContext context)
        {
            _context = context;
        }

        public async Task<User?> CreateAsync(UserDto dto)
        {
            var name = dto.Name.Trim();
            var email = dto.Email.Trim();
            var password = dto.Password.Trim();
            var role = dto.Role.Trim();

            var user = new User
            {
                Name = name,
                Email = email,
                Password = password,
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(int id, UserDto dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return null;

            user.Name = dto.Name.Trim();
            user.Email = dto.Email.Trim();
            //user.Password = dto.Password.Trim();
            user.Role = dto.Role.Trim();

           if (!string.IsNullOrEmpty(user.Password))
           {
               user.Password = dto.Password.Trim();
           }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserVm?> GetVmAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            return new UserVm
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = "",
                Role = user.Role
            };
        }

        public async Task<List<UserVm>> GetAllVmsAsync()
        {
            return await _context.Users
                .Select(u => new UserVm
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role
                })
                .ToListAsync();
        }

    }
}