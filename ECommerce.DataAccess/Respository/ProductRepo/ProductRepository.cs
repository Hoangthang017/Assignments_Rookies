using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Entities;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request;
using ECommerce.Models.ViewModels;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Respository.ProductRepo
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ECommerceDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ProductRepository(ECommerceDbContext context, IWebHostEnvironment webHostEnvironment, IMapper mapper) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task<int> Create(CreateProductRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Quantity = request.Quantity,
                ViewCount = 0,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Detail = request.Detail,
                        LanguageId = request.LanguageId
                    }
                }
            };

            if (request.Image != null)
            {
                // get www root
                string wwwwRootPath = _webHostEnvironment.WebRootPath;

                // get image name, path, save path
                var fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwwRootPath, @"images\products");
                var extension = Path.GetExtension(request.Image.FileName);

                // save image
                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    await request.Image.CopyToAsync(fileStream);
                }

                // add product images
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                     {
                         Caption = "Thumbnail image",
                         DateCreated = DateTime.Now,
                         FileSize = request.Image.Length,
                         ImagePath = @"\images\products\" + fileName + extension,
                         IsDefault = true,
                         ProductId = product.Id,
                     }
                };
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<bool> Update(UpdateProductRequest request)
        {
            // find product and product tranlastion
            var product = await _context.Products.FindAsync(request.ProductId);
            var productTranslation = await _context.ProductTranslations
                                            .Where(x => x.LanguageId == request.LanguageId && x.ProductId == request.ProductId)
                                            .FirstOrDefaultAsync();
            // check null
            if (product == null || productTranslation == null)
            {
                throw new ECommerceException($"Cannot find a product with id: {request.ProductId} and LanguageId: {request.LanguageId}");
            }

            // change database
            productTranslation.Name = request.Name;
            productTranslation.Description = request.Description;
            productTranslation.Detail = request.Detail;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}