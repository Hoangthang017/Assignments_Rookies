using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Request.Users
{
    public class RegisterRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; } = false;
    }
}