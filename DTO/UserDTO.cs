namespace Hotel.DTO
{
    public record UserDto(
        int Id,
        string Name,
        string Email,
        string Password,
        string Role
        );
}
