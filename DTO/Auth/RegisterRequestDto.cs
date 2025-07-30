namespace Hotel.DTO.Auth
{
    public record RegisterRequestDto(
        string Email, 
        string Password, 
        string Role
        );

}
