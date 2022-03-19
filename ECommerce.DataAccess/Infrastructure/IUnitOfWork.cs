using ECommerce.DataAccess.Repository.ImageRepo;
using ECommerce.DataAccess.Repository.OrderRepo;
using ECommerce.DataAccess.Repository.ProductRepo;
using ECommerce.DataAccess.Repository.UserRepo;

namespace ECommerce.DataAccess.Respository.Common
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }

        ICategoryRepository Category { get; }

        IUserRepository User { get; }

        IImageRepository Image { get; }

        IOrderRepository Order { get; }

        // save method
        Task<bool> Save();
    }
}