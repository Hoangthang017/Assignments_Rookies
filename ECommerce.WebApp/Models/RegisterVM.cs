using System.ComponentModel.DataAnnotations;

namespace ECommerce.WebApp.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "User name is required")]
        [MinLength(6, ErrorMessage = "User name at least 6 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"(?=^.{8,}$)(?=.*\d)(?=.*[!@#$%^&*]+)(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$",
            ErrorMessage = "Should contain at least a capital letter, a small letter, a small letter, a small letter and greater than or equal to 8")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "confirm password is not match")]
        public string ConfirmPassword { get; set; }
    }
}