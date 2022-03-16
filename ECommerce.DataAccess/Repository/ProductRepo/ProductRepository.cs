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
using Microsoft.EntityFrameworkCore;

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
            product.UpdatedDate = DateTime.Now;
            product.ProductTranslations = new List<ProductTranslation>() { productTranslation };

            // find category
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                throw new ECommerceException("Cannot find the category");
            var productInCategory = new ProductInCategory()
            {
                Category = category,
                CategoryId = request.CategoryId,
                Product = product,
                ProductId = product.Id
            };

            // add product to DbContext and Save
            await _context.Products.AddAsync(product);
            await _context.ProductInCategories.AddAsync(productInCategory);
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

        public async Task<PageResult<ProductViewModel>> GetAllPaging(string languageId, GetProductPagingRequest request)
        {
            // get product from tables
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where pt.LanguageId == languageId
                        select new { p, pt, pic, ct };
            // filter
            //if (!string.IsNullOrEmpty(request.Keyword))
            //{
            //    query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            //}

            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }

            // paging
            int totalRow = await query.CountAsync();
            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                            .Take(request.PageSize);

            var productVMs = await data.Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                IsShowOnHome = x.p.IsShowOnHome,
                ViewCount = x.p.ViewCount,
                CreatedDate = x.p.CreatedDate.ToShortDateString(),
                UpdatedDate = x.p.UpdatedDate.ToShortDateString(),
                OriginalPrice = x.p.OriginalPrice,
                Price = x.p.Price,
                Stock = x.p.Stock,
                SeoAlias = x.pt.SeoAlias,
                Description = x.pt.Description,
                Details = x.pt.Details,
                LanguageId = x.pt.LanguageId,
                Name = x.pt.Name,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                categoryId = x.ct.CategoryId,
                CategoryName = x.ct.Name,
                imagePaths = new List<string>(),
            }).ToListAsync();

            foreach (var productVM in productVMs)
            {
                var imagePaths = await GetImagePaths(productVM.Id);
                productVM.imagePaths = imagePaths;
            }

            var check = productVMs.ToList();

            // select and projection
            var pageResult = new PageResult<ProductViewModel>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow,
                Items = productVMs
            };

            await _context.SaveChangesAsync();

            return pageResult;
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            // get product in tabels
            var query = await (from p in _context.Products
                               join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                               join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                               join c in _context.Categories on pic.CategoryId equals c.Id
                               join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                               where p.Id == productId && pt.LanguageId == languageId && ct.LanguageId == languageId
                               select new { p, pt, pic, ct }).FirstOrDefaultAsync();

            // check null
            if (query == null)
                throw new ECommerceException("Cannot find product");

            // map to product view model
            var productVM = new ProductViewModel()
            {
                Id = query.p.Id,
                IsShowOnHome = query.p.IsShowOnHome,
                CreatedDate = query.p.CreatedDate.ToShortDateString(),
                OriginalPrice = query.p.OriginalPrice,
                Price = query.p.Price,
                Stock = query.p.Stock,
                UpdatedDate = query.p.UpdatedDate.ToShortDateString(),
                ViewCount = query.p.ViewCount,
                SeoAlias = query.pt.SeoAlias,
                Description = query.pt.Description,
                Details = query.pt.Details,
                LanguageId = query.pt.LanguageId,
                Name = query.pt.Name,
                SeoDescription = query.pt.SeoDescription,
                SeoTitle = query.pt.SeoTitle,
                categoryId = query.ct.CategoryId,
                CategoryName = query.ct.Name,
                imagePaths = new List<string>(),
            };

            productVM.imagePaths = await GetImagePaths(productVM.Id);

            return productVM;
        }

        public async Task<int> Update(int productId, string languageId, UpdateProductRequest request)
        {
            var query = await (from p in _context.Products
                               join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                               where p.Id == productId && pt.LanguageId == languageId
                               select new { p, pt }).FirstOrDefaultAsync();
            if (query == null)
                throw new ECommerceException("Cannot find product");
            _mapper.Map(request, query.pt);
            query.p.UpdatedDate = DateTime.Now;
            query.p.IsShowOnHome = request.IsShowOnHome;
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new ECommerceException("Cannot find product");
            product.Price = newPrice;
            product.UpdatedDate = DateTime.Now;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int Quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new ECommerceException("Cannot find product");
            product.Stock = Quantity;
            product.UpdatedDate = DateTime.Now;
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

        private async Task<List<string>> GetImagePaths(int productId)
        {
            var defaultImage = "https://localhost:7195/user-content/product/default-product-image.png";
            var imageProduct = await (from pi in _context.ProductImages
                                      join i in _context.Images on pi.ImageId equals i.Id
                                      where pi.ProductId == productId
                                      orderby pi.IsDefault descending
                                      select (i.ImagePath)).ToListAsync();
            return imageProduct.Count == 0 ? new List<string>() { defaultImage } : imageProduct;
        }

        public async Task<bool> DeleteRange(List<int> productIds)
        {
            var products = _context.Products.Where(x => productIds.Contains(x.Id)).ToList();
            _context.Products.RemoveRange(products);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ProductViewModel>> GetFeaturedProduct(string languageId, int take, int categoryId)
        {
            // get product from tables
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where pt.LanguageId == languageId && p.IsShowOnHome == true
                        select new { p, pt, pic, ct };

            // filter
            if (categoryId != 0)
            {
                query = query.Where(x => x.ct.CategoryId == categoryId);
            }

            // get featured
            var data = query.Take(take);

            var productVMs = await data.Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                IsShowOnHome = x.p.IsShowOnHome,
                ViewCount = x.p.ViewCount,
                CreatedDate = x.p.CreatedDate.ToShortDateString(),
                UpdatedDate = x.p.UpdatedDate.ToShortDateString(),
                OriginalPrice = x.p.OriginalPrice,
                Price = x.p.Price,
                Stock = x.p.Stock,
                SeoAlias = x.pt.SeoAlias,
                Description = x.pt.Description,
                Details = x.pt.Details,
                LanguageId = x.pt.LanguageId,
                Name = x.pt.Name,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                categoryId = x.ct.CategoryId,
                CategoryName = x.ct.Name,
                imagePaths = new List<string>(),
            }).ToListAsync();

            // add image path
            foreach (var productVM in productVMs)
            {
                var imagePaths = await GetImagePaths(productVM.Id);
                productVM.imagePaths = imagePaths;
            }

            await _context.SaveChangesAsync();

            return productVMs;
        }

        public async Task<List<ProductViewModel>> GetRelatedProducts(string languageId, int productId, int categoryId, int take)
        {
            var productVMs = await (from p in _context.Products
                                    join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                                    join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                                    join ct in _context.CategoryTranslations on pic.CategoryId equals ct.CategoryId
                                    where p.Id != productId && pt.LanguageId == languageId && ct.CategoryId == categoryId
                                    select new ProductViewModel()
                                    {
                                        Id = p.Id,
                                        IsShowOnHome = p.IsShowOnHome,
                                        ViewCount = p.ViewCount,
                                        CreatedDate = p.CreatedDate.ToShortDateString(),
                                        UpdatedDate = p.UpdatedDate.ToShortDateString(),
                                        OriginalPrice = p.OriginalPrice,
                                        Price = p.Price,
                                        Stock = p.Stock,
                                        SeoAlias = pt.SeoAlias,
                                        Description = pt.Description,
                                        Details = pt.Details,
                                        LanguageId = pt.LanguageId,
                                        Name = pt.Name,
                                        SeoDescription = pt.SeoDescription,
                                        SeoTitle = pt.SeoTitle,
                                        categoryId = ct.CategoryId,
                                        CategoryName = ct.Name,
                                        imagePaths = new List<string>(),
                                    }).Take(take).ToListAsync();

            if (productVMs == null) throw new ECommerceException("List null");

            // add image path
            foreach (var productVM in productVMs)
            {
                var imagePaths = await GetImagePaths(productVM.Id);
                productVM.imagePaths = imagePaths;
            }

            return productVMs;
        }

        public async Task<List<ProductReviewViewModel>> GetAllProductReview(int productId)
        {
            var productReviewVMs = await (from pr in _context.ProductReviews
                                          join u in _context.Users on pr.CustomerId equals u.Id
                                          join ui in _context.UserImages on u.Id equals ui.userId into us
                                          from ui in us.DefaultIfEmpty()
                                          join i in _context.Images on ui.ImageId equals i.Id into uis
                                          from i in uis.DefaultIfEmpty()
                                          where pr.ProductId == productId
                                          orderby pr.ReviewDate
                                          select new ProductReviewViewModel()
                                          {
                                              Id = pr.Id,
                                              CustomerId = u.Id,
                                              Rating = pr.Rating,
                                              ReviewDate = pr.ReviewDate,
                                              Comment = pr.Comment,
                                              CustomerName = u.FirstName + u.LastName,
                                              customerAvatar = i.ImagePath == null ? SystemConstants.AppSettings.DefaultAvatart : i.ImagePath,
                                          }).ToListAsync();

            if (productReviewVMs == null) throw new ECommerceException("Fail to get product review");

            return productReviewVMs;
        }

        public async Task<int> CreateProductReview(int productId, string customerId, CreateProductReviewRequest request)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new ECommerceException("Cannot find the product");

            var user = await _context.Users.FindAsync(new Guid(customerId));
            if (user == null) throw new ECommerceException("Cannot find the user");

            var productReview = new ProductReview()
            {
                Comment = request.Comment,
                Customer = user,
                CustomerId = user.Id,
                Product = product,
                ProductId = product.Id,
                Rating = request.Rating,
                ReviewDate = DateTime.Now,
            };

            _context.ProductReviews.Add(productReview);
            await _context.SaveChangesAsync();
            return productReview.Id;
        }

        public async Task<ProductReviewViewModel> GetProductReviewById(int reviewId)
        {
            var reviewVM = await (from pr in _context.ProductReviews
                                  join u in _context.Users on pr.CustomerId equals u.Id
                                  join ui in _context.UserImages on u.Id equals ui.userId
                                  join i in _context.Images on ui.ImageId equals i.Id
                                  where pr.Id == reviewId
                                  select new ProductReviewViewModel()
                                  {
                                      Id = pr.Id,
                                      Comment = pr.Comment,
                                      Rating = pr.Rating,
                                      ReviewDate = pr.ReviewDate,
                                      CustomerId = u.Id,
                                      CustomerName = u.FirstName + u.LastName,
                                      customerAvatar = i.ImagePath == null ? SystemConstants.AppSettings.DefaultAvatart : i.ImagePath,
                                  }).FirstOrDefaultAsync();
            if (reviewVM == null) throw new ECommerceException("Cannot find the review");

            return reviewVM;
        }

        #endregion method to save image
    }
}