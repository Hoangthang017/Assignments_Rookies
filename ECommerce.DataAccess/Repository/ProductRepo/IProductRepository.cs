﻿using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Request;
using ECommerce.Models.Request.Common;
using ECommerce.Models.ViewModels;
using ECommerce.Models.ViewModels.Common;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<int> Create(CreateProductRequest request);

        Task<int> Update(int productId, string languageId, UpdateProductRequest request);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStock(int productId, int Quantity);

        Task<bool> UpdateViewCount(int productId);

        Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);
    }
}