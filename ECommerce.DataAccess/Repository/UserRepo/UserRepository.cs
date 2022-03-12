using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Infrastructure.Common;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Users;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.UserInfos;
using ECommerce.Utilities;
using IdentityModel.Client;
using IdentityServer4;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
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
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        private string AvatarDefault { get; }
        private string IdentityServerHost { get; }

        public UserRepository(
            ECommerceDbContext context,
            IdentityServerTools tools,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IStorageService storageService,
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
            _storageService = storageService;

            // get url defautl
            IdentityServerHost = _config.GetSection("IdentityServerHost").Value;
            AvatarDefault = _config.GetSection("AvatarDefault").Value;
        }

        public async Task<string> Authencate(LoginRequest request)
        {
            var client = new HttpClient();

            // discover endpoints from metadata
            var disco = await GetDiscoveryDocument(client, IdentityServerHost);

            // request token
            var tokenClient = new TokenClient(new HttpClient() { BaseAddress = new Uri(disco.TokenEndpoint) }, new TokenClientOptions { ClientId = request.ClientId, ClientSecret = request.ClientSecret });
            var tokenResponse = await tokenClient.RequestPasswordTokenAsync(request.UserName, request.Password, request.Scope);

            if (tokenResponse.IsError)
            {
                Console.Error.WriteLine(tokenResponse.Error);
                return null;
            }

            return tokenResponse.AccessToken;
        }

        public async Task<UserInfoResponse> GetUserInfo(string token)
        {
            var client = new HttpClient();

            // discover endpoints from metadata
            var disco = await GetDiscoveryDocument(client, IdentityServerHost);

            // get claims
            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = token.Split()[1]
            });

            // check invalid respone
            if (response.IsError)
                throw new ECommerceException(response.Error);

            return response;
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

        public async Task<UserInfoViewModel> GetById(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                throw new ECommerceException($"Cannot find user with id: {UserId}");

            var userInfoVM = ECommerceMapper.Map<UserInfoViewModel>(_mapper, user);

            // find roles
            var roles = await _userManager.GetRolesAsync(user);
            userInfoVM.Role = roles.FirstOrDefault();

            // find avatar
            var image = await GetImageByUserId(UserId);
            userInfoVM.avatarUrl = (image == null ? AvatarDefault : image.ImagePath);

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
                var image = await GetImageByUserId(users[i].Id.ToString());
                userInfoVMs[i].avatarUrl = (image == null ? AvatarDefault : image.ImagePath);
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
                // delete image
                var image = await GetImageByUserId(userId);

                if (image != null)
                {
                    _context.Images.Remove(image);

                    await _storageService.DeleteFileAsync(image.ImagePath);
                }

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

        public async Task<PageResult<UserInfoViewModel>> GetAllPaging(GetUserPagingRequest request)
        {
            // get list user
            var query = from u in _context.Users
                        join ur in _context.UserRoles on u.Id equals ur.UserId
                        join r in _context.Roles on ur.RoleId equals r.Id
                        join ui in _context.UserImages on u.Id equals ui.userId into us
                        from ui in us.DefaultIfEmpty()
                        join i in _context.Images on ui.ImageId equals i.Id into uis
                        from i in uis.DefaultIfEmpty()
                        select new { u, r, ImagePath = (i == null ? AvatarDefault : i.ImagePath) };

            // paging
            int totalRow = await query.CountAsync();
            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                            .Take(request.PageSize);

            // conver to viewmodel
            var userInfoVMs = data.Select(x => new UserInfoViewModel()
            {
                UserName = x.u.UserName,
                DateOfBirth = x.u.DateOfBirth.ToShortDateString(),
                Email = x.u.Email,
                FirstName = x.u.FirstName,
                LastName = x.u.LastName,
                Id = x.u.Id.ToString(),
                Name = x.u.LastName + " " + x.u.FirstName,
                PhoneNumber = x.u.PhoneNumber,
                Role = x.r.Name,
                avatarUrl = x.ImagePath
            });

            // select and projection
            var pageResult = new PageResult<UserInfoViewModel>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow,
                Items = userInfoVMs
            };

            return pageResult;
        }

        #region method utilities

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName, "user");
            return fileName;
        }

        private async Task<Image> GetImageByUserId(string userId)
        {
            return await (from ui in _context.UserImages
                          join i in _context.Images on ui.ImageId equals i.Id
                          where ui.userId.ToString() == userId
                          select (i)).FirstOrDefaultAsync();
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

        #endregion method utilities
    }
}