using ECommerce.Models.Request.Users;
using ECommerce.Models.ViewModels.UserInfos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ECommerce.ApiItegration
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        private Dictionary<string, object> loginRequest { get; set; }
        private Dictionary<string, object> registerRequest { get; set; }

        private readonly string baseApiUrl = "api/Users";

        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            loginRequest = new Dictionary<string, object>()
            {
                {"clientId","web-app" },
                {"clientSecret","49C1A7E1-0C79-4A89-A3D6-A37998FB86B0" },
                {"scope", "openid profile phone email" }
            };

            registerRequest = new Dictionary<string, object>()
            {
                {"firstName","" },
                {"lastName","" },
                {"dateOfBirth", DateTime.Now } ,
                {"email","" },
                {"phoneNumber", "" },
                {"isAdmin", false }
            };
        }

        public async Task<Dictionary<string, string>> Authenticate(LoginRequest request)
        {
            loginRequest.Add("userName", request.UserName);
            loginRequest.Add("password", request.Password);
            loginRequest.Add("rememberMe", request.RememberMe);
            return await PostAsync<Dictionary<string, string>>(Path.Combine(baseApiUrl, "authenticate"), loginRequest);
        }

        public async Task<UserInfoViewModel> GetAccountInfor()
        {
            return await GetAsync<UserInfoViewModel>(Path.Combine(baseApiUrl, "account"), true);
        }

        public async Task RevokeToken()
        {
            await PostAsync(Path.Combine(baseApiUrl, "revoke"), loginRequest, true);
        }

        public async Task<UserInfoViewModel> Register(string userName, string password)
        {
            registerRequest.Add("userName", userName);
            registerRequest.Add("password", password);
            return await PostAsync<UserInfoViewModel>(Path.Combine(baseApiUrl, "register"), registerRequest);
        }
    }
}