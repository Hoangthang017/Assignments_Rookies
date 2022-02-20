using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository
{
    public interface IProductCategoryRepository
    {
        IEnumerable<ProductCategory> GetByAlias(string alias);
    }
}