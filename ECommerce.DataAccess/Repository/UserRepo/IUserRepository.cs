using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Users;
using ECommerce.Models.ViewModels.UserInfos;
using System.Security.Claims;

namespace ECommerce.DataAccess.Repository.UserRepo
{
    public interface IUserRepository : IRepository<User>
    {
        Task<string> Authencate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);

        Task<IEnumerable<Claim>> GetUserInfo(string token);
    }
}