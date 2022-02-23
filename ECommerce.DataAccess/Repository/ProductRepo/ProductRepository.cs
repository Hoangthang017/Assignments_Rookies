using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Products;
using ECommerce.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ECommerceDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ECommerceDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(CreateProductRequest request)
        {
            // map request to product and product translation
            var product = ECommerceMapper.Map<Product>(_mapper, request);
            ProductTranslation productTranslation = ECommerceMapper.Map<ProductTranslation>(_mapper, request);

            // create default value for product
            product.CreatedDate = DateTime.Now;
            product.ProductTranslations = new List<ProductTranslation>() { productTranslation };

            // add product to DbContext and Save
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();
        }

        public Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(int productId, string languageId, UpdateProductRequest request)
        {
            var productTranslation = await _context.ProductTranslations
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);
            if (productTranslation == null)
                throw new ECommerceException("Cannot find product");
            _mapper.Map(request, productTranslation);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new ECommerceException("Cannot find product");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int Quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new ECommerceException("Cannot find product");
            product.Stock = Quantity;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new ECommerceException("Cannot find product");
            product.ViewCount += 1;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}