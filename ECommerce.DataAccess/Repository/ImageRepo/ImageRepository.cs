using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Infrastructure.Common;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Images;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Images;
using ECommerce.Models.ViewModels.Slides;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace ECommerce.DataAccess.Repository.ImageRepo
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private readonly ECommerceDbContext _context;

        private readonly IStorageService _storageService;

        public ImageRepository(ECommerceDbContext context, IStorageService storageService) : base(context)
        {
            _context = context;
            _storageService = storageService;
        }

        #region User

        public async Task<int> AddImage(CreateUserImageRequest request)
        {
            if (request.ImageFile == null)
                throw new ECommerceException("Dont have file to create");

            var image = await AddInforImage(request, "user");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == request.UserId);

            if (user == null)
                throw new ECommerceException("Cannot find user");

            _context.UserImages.Add(new UserImage()
            {
                userId = user.Id,
                User = user,
                Image = image,
                ImageId = image.Id,
            });

            await _context.SaveChangesAsync();
            return image.Id;
        }

        public async Task<string> GetUserImagePathByUserId(string userId)
        {
            var urlPath = await (from u in _context.Users
                                 join ui in _context.UserImages on u.Id equals ui.userId into ps
                                 from ui in ps.DefaultIfEmpty()
                                 join i in _context.Images on ui.ImageId equals i.Id into uis
                                 from i in uis.DefaultIfEmpty()
                                 where u.Id.ToString() == userId
                                 select new { ImagePath = (i == null ? SystemConstants.ImageSettings.DefaultAvatart : i.ImagePath) }).FirstOrDefaultAsync();

            if (urlPath == null) throw new ECommerceException("Have error in process query");

            return urlPath.ImagePath;
        }

        public async Task<bool> UpdateImage(UpdateUserImageRequest request)
        {
            var query = (from u in _context.Users
                         join ui in _context.UserImages on u.Id equals ui.userId into ps
                         from ui in ps.DefaultIfEmpty()
                         join i in _context.Images on ui.ImageId equals i.Id into uis
                         from i in uis.DefaultIfEmpty()
                         where u.Id.ToString() == request.UserId
                         select new { u, i }).FirstOrDefault();

            if (query == null) throw new ECommerceException("Cannot not find the image");

            if (query.i != null)
                await DeleteImage(query.i.Id);

            var image = await AddInforImage(new BaseImageRequest()
            {
                Caption = request.Caption,
                ImageFile = request.ImageFile,
                SortOrder = request.SortOrder,
            }, SystemConstants.ImageSettings.FolderSaveUserImage);

            await _context.UserImages.AddAsync(new UserImage()
            {
                userId = query.u.Id,
                User = query.u,
                Image = image,
                ImageId = image.Id,
            });

            return await _context.SaveChangesAsync() > 0;
        }

        #endregion User

        #region Product

        public async Task<int> AddImage(CreateProductImageRequest request)
        {
            if (request.ImageFile == null)
                throw new ECommerceException("Dont have file to create");

            var image = await AddInforImage(request, SystemConstants.ImageSettings.FolderSaveProductImage);

            // get product
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId);
            if (product == null)
                throw new ECommerceException("Cannot find product");

            // remove old thumbnail if have new thumbnail
            if (request.IsDefault)
            {
                var thumbnail = _context.ProductImages.Where(x => x.ProductId == request.ProductId && x.IsDefault == true);
                await thumbnail.ForEachAsync(x => x.IsDefault = false);
            }

            _context.ProductImages.Add(new ProductImage()
            {
                ProductId = product.Id,
                Image = image,
                ImageId = image.Id,
                IsDefault = request.IsDefault
            });

            await _context.SaveChangesAsync();
            return image.Id;
        }

        public async Task<PageResult<ImageViewModel>> GetAllPaging(int productId, PagingRequestBase request)
        {
            var imageProduct = (from pi in _context.ProductImages
                                join i in _context.Images on pi.ImageId equals i.Id
                                where pi.ProductId == productId
                                orderby pi.IsDefault descending
                                select new ImageViewModel()
                                {
                                    Caption = i.Caption,
                                    Id = i.Id,
                                    ImagePath = i.ImagePath,
                                    IsDefault = pi.IsDefault
                                });

            // get image if dont have image => null
            if (imageProduct == null) return new PageResult<ImageViewModel>();

            int totalRow = await imageProduct.CountAsync();
            var data = imageProduct.Skip((request.PageIndex - 1) * request.PageSize)
                            .Take(request.PageSize);

            // select and projection
            var pageResult = new PageResult<ImageViewModel>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow,
                Items = data
            };

            return pageResult;
        }

        public async Task<bool> UpdateImage(int imageId, int productId, UpdateProductImageRequest request)
        {
            // get all image of product
            var images = (from pi in _context.ProductImages
                          join i in _context.Images on pi.ImageId equals i.Id
                          where pi.ProductId == productId
                          select new { pi, i });

            if (images == null) throw new ECommerceException("Product don't have any image");

            // remove old image default
            if (request.IsDefault)
            {
                var oldDefaultImage = images.Where(x => x.pi.IsDefault == true);
                if (oldDefaultImage != null)
                {
                    await oldDefaultImage.ForEachAsync(x => x.pi.IsDefault = false);
                }
            }

            // update new value for image
            var image = await images.Where(x => x.i.Id == imageId).FirstOrDefaultAsync();
            if (image == null) throw new ECommerceException("Cannot find the image");

            image.i.Caption = request.Caption;
            image.i.SortOrder = request.SortOrder;
            image.pi.IsDefault = request.IsDefault;

            return await _context.SaveChangesAsync() > 0;
        }

        #endregion Product

        #region Base

        public async Task<bool> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
                throw new ECommerceException("Cannot find the image");

            _context.Images.Remove(image);

            await _storageService.DeleteFileAsync(image.ImagePath);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<SlideViewModel>> GetAllSlide(int take)
        {
            var slideVMs = from i in _context.Images
                           join s in _context.Slides on i.Id equals s.ImageId
                           select new SlideViewModel()
                           {
                               Id = i.Id,
                               Description = s.Description,
                               ImagePath = i.ImagePath,
                               Name = i.Caption,
                               SortOrder = i.SortOrder,
                           };

            // dont have slide
            if (slideVMs == null) return new List<SlideViewModel>();

            return await slideVMs.Take(take).ToListAsync();
        }

        private async Task<Image> AddInforImage(BaseImageRequest request, string type)
        {
            return new Image()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                FileSize = request.ImageFile.Length,
                ImagePath = await this.SaveFile(request.ImageFile, type),
                SortOrder = 1
            };
        }

        private async Task<string> SaveFile(IFormFile file, string type)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName, type);
            return _storageService.GetFileUrl(fileName, type);
        }

        #endregion Base
    }
}