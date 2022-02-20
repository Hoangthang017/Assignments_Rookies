using ECommerce.DataAccess.Repository.CommonRepo;
using ECommerce.DataAccess.Repository.OrderRepo;
using ECommerce.DataAccess.Repository.ProductRepo;

namespace ECommerce.DataAccess.Respository.Common
{
    public interface IUnitOfWork
    {
        IFooterRepository Footers { get; }

        IProductRepository Products { get; }

        IPostRepository Posts { get; }

        Task Save();
    }
}