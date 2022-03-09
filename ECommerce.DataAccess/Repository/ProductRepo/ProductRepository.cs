using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Infrastructure.Common;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Products;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ECommerceDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public ProductRepository(ECommerceDbContext context, IStorageService storageService, IMapper mapper) : base(context)
        {
            _context = context;
            _storageService = storageService;
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

            // handler image
            if (request.Image != null)
            {
                var image = new Image()
                {
                    Caption = "Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.Image.Length,
                    ImagePath = await this.SaveFile(request.Image),
                    SortOrder = 1
                };

                product.ProductImages = new List<ProductImage>()
                {
                  new ProductImage()
                  {
                      ProductId = product.Id,
                      Product = product,
                      Image = image,
                      ImageId = image.Id,
                  }
                };
            }

            // add product to DbContext and Save
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<bool> Delete(int productId)
        {
            // find product
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new ECommerceException("Cannot find product");

            // find all image of product and delete it out wwwroot
            var images = from p in _context.Products
                         join pi in _context.ProductImages on p.Id equals pi.ProductId
                         join i in _context.Images on pi.ImageId equals i.Id
                         where p.Id == productId
                         select i;

            foreach (Image image in images.ToList())
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            // get product from tables
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic, c };
            // filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }

            // paging
            int totalRow = await query.CountAsync();
            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                            .Take(request.PageSize);
            var productVMs = ECommerceMapper.Map<List<ProductViewModel>>(_mapper,
                data.Select(x => x.p),
                data.Select(x => x.pt));

            // select and projection
            var pageResult = new PageResult<ProductViewModel>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow,
                Items = productVMs
            };

            return pageResult;
        }

        public async Task<ProductViewModel> GetById(int productId)
        {
            // get product in tabels
            var query = await (from p in _context.Products
                               join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                               join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                               where p.Id == productId
                               select new { p, pt, pic }).FirstOrDefaultAsync();

            // check null
            if (query == null)
                throw new ECommerceException("Cannot find product");

            // map to product view model
            var productVM = ECommerceMapper.Map<ProductViewModel>(_mapper, query.p, query.pt, query.pic);

            return productVM;
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

        #region method to save image

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        #endregion method to save image
    }
}