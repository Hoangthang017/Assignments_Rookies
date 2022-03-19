using ECommerce.Models.ViewModels.Categories;

namespace ECommerce.ApiItegration
{
    public interface ICategoryApiClient
    {
        Task<List<BaseCategoryViewModel>> GetFeaturedCategory(string languageId, int take);

        Task<List<BaseCategoryViewModel>> GetActiveCategory(string languageId);
    }
}