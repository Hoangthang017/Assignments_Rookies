using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Images;
using ECommerce.Models.ViewModels.ProductImages;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public interface IProductImageRepository : IRepository<Image>
    {
    }
}