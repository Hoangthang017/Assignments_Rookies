using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ApiItegration
{
    public interface IProductApiClient
    {
        Task<List<ProductViewModel>> GetFeaturedProduct(int categoryId, int take, string languageId);

        Task<ProductViewModel> GetProductById(string languageId, int productId);

        Task<List<ProductViewModel>> GetRelatedProduct(string languageId, int productId, int categoryId, int take);

        Task<PageResult<ProductViewModel>> GetAllPaging(string languageId, GetProductPagingRequest request);

        Task<List<ProductReviewViewModel>> GetAllReview(int productId);

        Task<ProductReviewViewModel> CreateProductReview(int productId, string customerId, CreateProductReviewRequest request);

        Task<bool> UpdateViewCount(int productId);
    }
}