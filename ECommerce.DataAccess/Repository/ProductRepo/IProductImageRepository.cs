using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.ProductImages;
using ECommerce.Models.ViewModels.ProductImages;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public interface IProductImageRepository : IRepository<Image>
    {
        Task<int> AddImage(int productId, CreateImageRequest request);

        Task<int> UpdateImage(int imageId, UpdateImageRequest request);

        Task<int> RemoveImage(int imageId);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetAllImages(int productId);
    }
}