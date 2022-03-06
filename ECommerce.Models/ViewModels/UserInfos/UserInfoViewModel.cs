namespace ECommerce.Models.ViewModels.UserInfos
{
    public class UserInfoViewModel
    {
        public string Sub { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public List<string> Roles { get; set; }
    }
}