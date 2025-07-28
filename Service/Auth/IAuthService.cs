using Hotel.DTO;
using Hotel.Models;
using Hotel.Models.ViewModels.Auth;

namespace Hotel.Service.Auth
{
    public interface IAuthService
    {
        Task<(bool, string? Error)> RegisterAsync(RegisterVm vm);
        Task<(bool, User? User, string? Error)> ValidateCredentialsAsync(string email, string password);

        Task<LoginResult> BuildLoginAsync(string email, string password);
    }
}
