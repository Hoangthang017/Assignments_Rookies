namespace ECommerce.Models.Request.Users
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsAdmin { get; set; } = false;
    }
}