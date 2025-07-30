using System.Security.Claims;

namespace Hotel.DTO.Auth
{
    public record LoginResultDto(bool Success, ClaimsPrincipal Principal, string? RedirectUrl, string? Error);
}
