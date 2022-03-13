using ECommerce.Models.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ApiItegration
{
    public interface ICategoryApiClient
    {
        Task<List<BaseCategoryViewModel>> GetFeaturedCategory(string languageId, int take);
    }
}