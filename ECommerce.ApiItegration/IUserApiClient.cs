using ECommerce.Models.Request.Users;
using ECommerce.Models.ViewModels.UserInfos;

namespace ECommerce.ApiItegration
{
    public interface IUserApiClient
    {
        Task<Dictionary<string, string>> Authenticate(LoginRequest request);

        Task RevokeToken();

        Task<UserInfoViewModel> GetAccountInfor();

        Task<UserInfoViewModel> Register(string userName, string password);
    }
}