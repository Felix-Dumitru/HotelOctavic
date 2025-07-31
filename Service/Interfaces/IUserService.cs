using Hotel.DTO;
using Hotel.Models;
using Hotel.Models.ViewModels;

namespace Hotel.Service.Interfaces
{
    public interface IUserService
    {
        Task<User?> CreateAsync(UserDto dto);
        Task<User?> UpdateAsync(int id, UserDto dto);
        Task<bool> DeleteAsync(int id);

        Task<UserVm?> GetVmAsync(int id);
        Task<List<UserVm>> GetAllVmsAsync();
    }
}
