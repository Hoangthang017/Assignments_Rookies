using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Infrastructure.Common;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Images;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
                var image = await AddInforImage(request);

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

        public async Task<bool> DeleteImage(int id)
        {
            // delete image
            var image = await _context.Images.FindAsync(id);

            if (image != null)
            {
                _context.Images.Remove(image);

                await _storageService.DeleteFileAsync(image.ImagePath);

                return true;
            }

            throw new ECommerceException("Cannot find the image");
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
                });

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

        private async Task<Image> AddInforImage(CreateImageBaseRequest request)
        {
            return new Image()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                FileSize = request.ImageFile.Length,
                ImagePath = await this.SaveFile(request.ImageFile),
                SortOrder = 1
            };
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName, "user");
            return _storageService.GetFileUrl(fileName, "user");
        }
    }
}