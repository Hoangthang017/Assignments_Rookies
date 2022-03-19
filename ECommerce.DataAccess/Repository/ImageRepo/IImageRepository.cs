using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Images;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Images;
using ECommerce.Models.ViewModels.Slides;

namespace ECommerce.DataAccess.Repository.ImageRepo
{
    public interface IImageRepository : IRepository<Image>
    {
        #region user

        Task<int> AddImage(CreateUserImageRequest request);

        Task<string> GetUserImagePathByUserId(string userId);

        Task<bool> UpdateImage(UpdateUserImageRequest request);

        #endregion user

        #region Product

        Task<int> AddImage(CreateProductImageRequest request);

        Task<PageResult<ImageViewModel>> GetAllPaging(int productId, PagingRequestBase request);

        Task<bool> UpdateImage(int imageId, int productId, UpdateProductImageRequest request);

        #endregion Product

        #region Base

        Task<List<SlideViewModel>> GetAllSlide(int take);

        Task<bool> DeleteImage(int id);

        #endregion Base
    }
}