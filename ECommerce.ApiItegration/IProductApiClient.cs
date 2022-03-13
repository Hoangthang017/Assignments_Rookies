using ECommerce.Models.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ApiItegration
{
    public interface IProductApiClient
    {
        Task<List<ProductViewModel>> GetFeaturedProduct(int categoryId, int take, string languageId);
    }
}