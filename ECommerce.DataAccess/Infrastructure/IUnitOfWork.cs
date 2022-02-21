using ECommerce.DataAccess.Repository.ProductRepo;

namespace ECommerce.DataAccess.Respository.Common
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }

        Task<bool> Save();
    }
}