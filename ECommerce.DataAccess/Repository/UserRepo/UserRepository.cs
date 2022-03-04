using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Users;
using IdentityModel.Client;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ECommerce.DataAccess.Repository.UserRepo
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ECommerceDbContext _context;
        private readonly IdentityServerTools _tools;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _config;

        public UserRepository(
            ECommerceDbContext context,
            IdentityServerTools tools,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IConfiguration config) : base(context)
        {
            _context = context;
            _tools = tools;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<string> Authencate(LoginRequest request)
        {
            var client = new HttpClient();

            // discover endpoints from metadata
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            disco.Policy.ValidateIssuerName = false;
            if (disco.IsError)
            {
                return (disco.IsError + disco.Error);
            }

            // request token
            var tokenClient = new TokenClient(new HttpClient() { BaseAddress = new Uri(disco.TokenEndpoint) }, new TokenClientOptions { ClientId = request.ClientId, ClientSecret = request.ClientSecret });
            var tokenResponse = await tokenClient.RequestPasswordTokenAsync(request.UserName, request.Password, request.Scope);

            if (tokenResponse.IsError)
            {
                Console.Error.WriteLine(tokenResponse.Error);
                return null;
            }

            //Console.WriteLine(tokenResponse.Json);
            //Console.WriteLine("\n\n");
            return tokenResponse.AccessToken;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var user = new User()
            {
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
                return true;
            return false;
        }

        public async Task<string> GetAccessToken()
        {
            var token = await _tools.IssueClientJwtAsync(
                clientId: "BackendApi",
                lifetime: 3600,
                audiences: new[] { "api.BackendApi" }
            );
            return token.ToString();
        }
    }
}