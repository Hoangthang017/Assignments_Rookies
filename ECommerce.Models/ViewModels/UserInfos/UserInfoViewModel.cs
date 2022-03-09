namespace ECommerce.Models.ViewModels.UserInfos
{
    public class UserInfoViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateOfBirth { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        public string avatarUrl { get; set; } =
            "https://localhost:7195/user-content/user/smapleAvatar.jpg";
    }
}