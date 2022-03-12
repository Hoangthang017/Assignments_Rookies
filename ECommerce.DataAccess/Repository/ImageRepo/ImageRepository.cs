using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Infrastructure.Common;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Images;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Images;
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

        public async Task<int> AddImage(CreateUserImageRequest request)
        {
            if (request.ImageFile != null)
            {
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

            return 0;
        }

        public async Task<int> AddImage(CreateProductImageRequest request)
        {
            if (request.ImageFile != null)
            {
                var image = await AddInforImage(request, "product");

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

            return 0;
        }

        public async Task<bool> DeleteImage(int id)
        {
            // delete image
            var image = await _context.Images.FindAsync(id);

            if (image != null)
            {
                _context.Images.Remove(image);

                await _storageService.DeleteFileAsync(image.ImagePath);

                return await _context.SaveChangesAsync() > 0;
            }

            throw new ECommerceException("Cannot find the image");
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

            // paging
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

        public async Task<string> GetUserImagePathByUserId(string userId)
        {
            var AvatarDefault = "https://localhost:7195/user-content/user/user-default.jpg";
            // delte old image
            var urlPath = await (from u in _context.Users
                                 join ui in _context.UserImages on u.Id equals ui.userId into ps
                                 from ui in ps.DefaultIfEmpty()
                                 join i in _context.Images on ui.ImageId equals i.Id into uis
                                 from i in uis.DefaultIfEmpty()
                                 where u.Id.ToString() == userId
                                 select new { ImagePath = (i == null ? AvatarDefault : i.ImagePath) }).FirstOrDefaultAsync();

            return urlPath.ImagePath;
        }

        public async Task<bool> UpdateImage(UpdateUserImageRequest request)
        {
            // delte old image
            var query = (from u in _context.Users
                         join ui in _context.UserImages on u.Id equals ui.userId into ps
                         from ui in ps.DefaultIfEmpty()
                         join i in _context.Images on ui.ImageId equals i.Id into uis
                         from i in uis.DefaultIfEmpty()
                         where u.Id.ToString() == request.UserId
                         select new { u, i }).FirstOrDefault();

            if (query != null)
            {
                if (query.i != null)
                    await DeleteImage(query.i.Id);

                var image = await AddInforImage(new CreateImageBaseRequest()
                {
                    Caption = request.Caption,
                    ImageFile = request.ImageFile,
                    SortOrder = request.SortOrder,
                }, "user");

                await _context.UserImages.AddAsync(new UserImage()
                {
                    userId = query.u.Id,
                    User = query.u,
                    Image = image,
                    ImageId = image.Id,
                });

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
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

        private async Task<Image> AddInforImage(CreateImageBaseRequest request, string type)
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
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName, type);
            return _storageService.GetFileUrl(fileName, type);

        }
    }
}