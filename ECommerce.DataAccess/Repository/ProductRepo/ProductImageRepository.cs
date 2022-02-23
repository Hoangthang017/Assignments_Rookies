using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Infrastructure.Common;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.ProductImages;
using ECommerce.Models.ViewModels.ProductImages;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly ECommerceDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;

        public ProductImageRepository(ECommerceDbContext context, IStorageService storageService, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, CreateProductImageRequest request)
        {
            // find the product contains image list
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new ECommerceException("Cannot find prouct");

            // remove
            if (request.IsDefault == true)
            {
                var defaultImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == productId);
                defaultImage.IsDefault = false;
            }

            // create product image
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                FileSize = request.ImageFile.Length,
                ImagePath = await this.SaveFile(request.ImageFile),
                IsDefault = request.IsDefault,
                SortOrder = 1,
            };

            // add product image
            if (product.ProductImages == null)
            {
                product.ProductImages = new List<ProductImage>() { productImage };
            }
            else
            {
                product.ProductImages.ToList().Add(productImage);
            }

            await _context.SaveChangesAsync();

            return productImage.Id;
        }

        public async Task<List<ProductImageViewModel>> GetAllImages(int productId)
        {
            var productImages = await _context.ProductImages.FindAsync(productId);
            if (productImages == null)
                throw new ECommerceException("product don't have image");
            var productImageVMs = ECommerceMapper.Map<List<ProductImageViewModel>>(_mapper, productImages);
            return productImageVMs;
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new ECommerceException("Cannot find image");
            var productImageVM = ECommerceMapper.Map<ProductImageViewModel>(_mapper, productImage);
            return productImageVM;
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new ECommerceException("Cannot find image");
            await _storageService.DeleteFileAsync(image.ImagePath);
            _context.ProductImages.Remove(image);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, UpdateProductImageRequest request)
        {
            var newImage = ECommerceMapper.Map<ProductImage>(_mapper, request);
            if (request.IsDefault == true)
            {
                var defaultImage = await _context.ProductImages.FindAsync(imageId);
                defaultImage.IsDefault = false;
            }
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new ECommerceException("Cannot find image");
            _mapper.Map(newImage, image);
            return await _context.SaveChangesAsync();
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