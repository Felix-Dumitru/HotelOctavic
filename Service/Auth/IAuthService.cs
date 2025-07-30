using Hotel.Models;
using Hotel.Models.ViewModels.Auth;
using Hotel.DTO.Auth;

namespace Hotel.Service.Auth
{
    public interface IAuthService
    {
        Task<(bool, string? Error)> RegisterAsync(RegisterRequestDto dto);
        Task<(bool, User? User, string? Error)> ValidateCredentialsAsync(string email, string password);

        Task<LoginResultDto> BuildLoginAsync(LoginRequestDto dto);
    }
}
