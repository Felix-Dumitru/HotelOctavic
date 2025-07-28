using System.ComponentModel.DataAnnotations;

namespace Hotel.Models.ViewModels.Auth
{
    public class RegisterVm
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
