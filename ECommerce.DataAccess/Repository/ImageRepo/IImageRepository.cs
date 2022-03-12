using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Images;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository.ImageRepo
{
    public interface IImageRepository : IRepository<Image>
    {
        Task<int> AddImage(CreateUserImageRequest request);

        Task<int> AddImage(CreateProductImageRequest request);

        Task<bool> UpdateImage(UpdateUserImageRequest request);

        Task<bool> UpdateImage(int imageId, int productId, UpdateProductImageRequest request);

        Task<PageResult<ImageViewModel>> GetAllPaging(int productId, PagingRequestBase request);

        Task<string> GetUserImagePathByUserId(string userId);

        Task<bool> DeleteImage(int id);
    }
}