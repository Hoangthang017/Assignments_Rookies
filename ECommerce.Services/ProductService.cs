using ECommerce.DataAccess.Repository.ProductRepo;
using ECommerce.DataAccess.Respository.Common;
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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public ProductService(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public Task<int> Create(CreateProductRequest request)
        {
            return _productRepository.Create(request);
        }

        public Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(int productId, string languageId, UpdateProductRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStock(int productId, int Quantity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateViewCount(int productId)
        {
            throw new NotImplementedException();
        }
    }
}