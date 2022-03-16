using System.ComponentModel.DataAnnotations;

namespace ECommerce.WebApp.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "confirm password is required")]
        [Compare("Password", ErrorMessage = "confirm password is not match")]
        public string ConfirmPassword { get; set; }
    }
}