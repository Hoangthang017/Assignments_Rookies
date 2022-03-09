using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Users;
using ECommerce.Models.ViewModels.UserInfos;
using ECommerce.Utilities;
using IdentityModel.Client;
using IdentityServer4;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace ECommerce.DataAccess.Repository.UserRepo
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ECommerceDbContext _context;
        private readonly IdentityServerTools _tools;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserRepository(
            ECommerceDbContext context,
            IdentityServerTools tools,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IMapper mapper,
            IConfiguration config) : base(context)
        {
            _context = context;
            _tools = tools;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _config = config;
        }

        public async Task<string> Authencate(LoginRequest request)
        {
            var client = new HttpClient();

            // discover endpoints from metadata
            var disco = await GetDiscoveryDocument(client, "https://localhost:5001");

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

        public async Task<string> CreateUser(RegisterRequest request)
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

            var userRespone = await _userManager.CreateAsync(user, request.Password);
            if (userRespone.Succeeded)
            {
                // add default role customer
                var roleRespone = await _userManager.AddToRoleAsync(user, "customer");
                if (roleRespone.Succeeded)
                    return user.Id.ToString();

                throw new ECommerceException(roleRespone.Errors.ToString());
            }

            throw new ECommerceException(userRespone.Errors.ToString());
        }

        public async Task<UserInfoResponse> GetUserInfo(string token)
        {
            var client = new HttpClient();

            // discover endpoints from metadata
            var disco = await GetDiscoveryDocument(client, "https://localhost:5001");

            // get claims
            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = token.Split()[1]
            });

            // check invalid respone
            if (response.IsError)
                throw new ECommerceException(response.Error);

            //var check = response;
            //var result = new Dictionary<string, string>();
            //response.Claims.ToList().ForEach(x => result.Add(x.Type, x.Value));

            return response;
        }

        private async Task<DiscoveryDocumentResponse> GetDiscoveryDocument(HttpClient client, string url)
        {
            // discover endpoints from metadata
            var disco = await client.GetDiscoveryDocumentAsync(url);
            disco.Policy.ValidateIssuerName = false;
            if (disco.IsError)
            {
                throw new ECommerceException(disco.IsError + disco.Error);
            }
            return disco;
        }

        public async Task<UserInfoViewModel> GetById(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                throw new ECommerceException($"Cannot find user with id: {UserId}");

            var roles = await _userManager.GetRolesAsync(user);

            var userInfoVM = ECommerceMapper.Map<UserInfoViewModel>(_mapper, user);
            userInfoVM.Role = roles.FirstOrDefault();
            return userInfoVM;
        }

        public async Task<IEnumerable<UserInfoViewModel>> GetAll()
        {
            var users = _userManager.Users.ToList();

            var userInfoVMs = ECommerceMapper.Map<List<UserInfoViewModel>>(_mapper, users);

            for (int i = 0; i < users.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(users[i]);
                userInfoVMs[i].Role = roles.FirstOrDefault();
            }

            return userInfoVMs;
        }

        public async Task<string> UpdateRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            // remove all old role -> customer role
            var flagRemoveAll = true;
            var oldRoles = await _userManager.GetRolesAsync(user);
            foreach (var oldRole in oldRoles)
            {
                var removeRespone = await _userManager.RemoveFromRoleAsync(user, oldRole);
                if (!removeRespone.Succeeded)
                {
                    flagRemoveAll = false;
                    break;
                }
            }

            if (flagRemoveAll)
            {
                // add admin role
                var result = await _userManager.AddToRoleAsync(user, role);
                if (result.Succeeded)
                {
                    return userId;
                }

                throw new ECommerceException(result.Errors.ToString());
            }

            throw new ECommerceException("Fail to remove old roles");
        }

        public async Task<bool> UpdateUser(string userId, UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.DateOfBirth = request.DateOfBirth;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            if (user != null)
            {
                await _userManager.UpdateAsync(user);
                var newRole = request.IsAdmin ? "admin" : "customer";
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains(newRole))
                {
                    await UpdateRole(userId, newRole);
                }
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return true;
            }
            return false;
        }

        public async Task<bool> RemoveRangeUser(List<string> userIds)
        {
            foreach (var userId in userIds)
            {
                var result = await RemoveUser(userId);
                if (!result)
                    return false;
            }
            return true;
        }
    }
}