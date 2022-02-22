using ECommerce.Models.Request;
using ECommerce.Models.Request.Common;
using ECommerce.Models.ViewModels;
using ECommerce.Models.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public interface IProductService
    {
        Task<int> Create(CreateProductRequest request);

        Task<int> Update(int productId, string languageId, UpdateProductRequest request);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStock(int productId, int Quantity);

        Task<bool> UpdateViewCount(int productId);

        Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);
    }
}