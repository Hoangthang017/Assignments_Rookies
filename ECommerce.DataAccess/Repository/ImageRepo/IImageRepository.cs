using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Images;
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

        Task<bool> UpdateImage(UpdateUserImageRequest request);

        Task<string> GetUserImagePathByUserId(string userId);

        Task<bool> DeleteImage(int id);
    }
}