using ECommerce.Models.Request.Users;
using ECommerce.Models.ViewModels.UserInfos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ECommerce.ApiItegration
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        private readonly string _clientId = "web-app";

        private readonly string _clientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";

        private readonly string _scope = "openid profile phone email";

        private readonly string _baseApiUrl = "api/Users";

        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<Dictionary<string, string>> Authenticate(LoginRequest request)
        {
            request.ClientId = _clientId;
            request.ClientSecret = _clientSecret;
            request.Scope = _scope;
            return await PostAsync<Dictionary<string, string>, LoginRequest>(Path.Combine(_baseApiUrl, "authenticate"), request);
        }

        public async Task<UserInfoViewModel> GetAccountInfor()
        {
            return await GetAsync<UserInfoViewModel>(Path.Combine(_baseApiUrl, "account"), true);
        }

        public async Task<UserInfoViewModel> Register(string userName, string password)
        {
            var registerRequest = new RegisterRequest() { UserName = userName, Password = password };
            return await PostAsync<UserInfoViewModel, RegisterRequest>(Path.Combine(_baseApiUrl, "register"), registerRequest);
        }
    }
}