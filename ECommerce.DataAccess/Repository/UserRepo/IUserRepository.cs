using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Users;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.UserInfos;
using IdentityModel.Client;

namespace ECommerce.DataAccess.Repository.UserRepo
{
    public interface IUserRepository : IRepository<User>
    {
        Task<string> Authencate(LoginRequest request);

        Task<string> CreateUser(RegisterRequest request);

        Task<UserInfoResponse> GetUserInfo(string token);

        Task<UserInfoViewModel> GetById(string UserId);

        Task<PageResult<UserInfoViewModel>> GetAllPaging(GetUserPagingRequest request);

        Task<string> UpdateRole(string userId, string role);

        Task<bool> UpdateUser(string userId, UpdateUserRequest request);

        Task<bool> RemoveUser(string userId);

        Task<bool> RemoveRangeUser(List<string> userIds);
    }
}