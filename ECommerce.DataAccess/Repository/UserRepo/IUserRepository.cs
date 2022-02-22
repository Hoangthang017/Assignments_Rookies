using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Users;

namespace ECommerce.DataAccess.Repository.UserRepo
{
    public interface IUserRepository : IRepository<User>
    {
        Task<string> Authencate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);
    }
}