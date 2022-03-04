using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Users;
using ECommerce.Utilities;
using IdentityModel.Client;
using IdentityServer4;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return null;
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return null;
            }

            var userToken = await GetAccessToken();

            //var roles = _userManager.GetRolesAsync(user);
            //var claims = new[]
            //{
            //    new Claim(ClaimTypes.Email, user.Email),
            //    new Claim(ClaimTypes.GivenName, user.FirstName),
            //    new Claim(ClaimTypes.Role, string.Join(";",roles))
            //};

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(_config["Tokens:Issuer"],
            //    _config["Tokens:Issuer"],
            //    claims,
            //    expires: DateTime.Now.AddHours(3),
            //    signingCredentials: creds);

            return userToken;

            //var client = new HttpClient();

            //// discover endpoints from metadata
            //var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            //disco.Policy.ValidateIssuerName = false;
            //if (disco.IsError)
            //{
            //    return Json(disco.IsError + disco.Error);
            //}

            //// request token
            //var tokenClient = new TokenClient(disco.TokenEndpoint, model.ClientId, model.ClientSecrets);
            //var tokenResponse = await tokenClient.RequestClientCredentialsAsync(model.Scope);

            //if (tokenResponse.IsError)
            //{
            //    return Json(tokenResponse.Error);
            //}

            //return Json(tokenResponse.Json);
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