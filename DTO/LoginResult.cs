using System.Security.Claims;

namespace Hotel.DTO
{
    public record LoginResult(bool Success, ClaimsPrincipal Principal, string? RedirectUrl, string? Error);
}
