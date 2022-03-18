using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Request.Users
{
    public class LoginRequest : InforClientRequest
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}