using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Products;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<int> Create(CreateProductRequest request);

        Task<PageResult<ProductViewModel>> GetAllPaging(string languageId, GetProductPagingRequest request);

        Task<ProductViewModel> GetById(int productId, string languageId);

        Task<int> Update(int productId, string languageId, UpdateProductRequest request);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStock(int productId, int Quantity);

        Task<bool> UpdateViewCount(int productId);

        Task<bool> Delete(int productId);

        Task<bool> DeleteRange(List<int> productIds);
    }
}