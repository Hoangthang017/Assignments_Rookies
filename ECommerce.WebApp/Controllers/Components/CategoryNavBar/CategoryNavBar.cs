using ECommerce.ApiItegration;
using ECommerce.Models.ViewModels.Categories;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Mvc;
using ECommerce.WebApp.Models;

namespace ECommerce.WebApp.Controllers.Components.CategoryNavBar
{
    [ViewComponent]
    public class CategoryNavBar : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public CategoryNavBar(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool isFeatured = false)
        {
            var categoryVM = new List<BaseCategoryViewModel>();
            var allRouteData = new Dictionary<string, string>();
            if (isFeatured)
            {
                categoryVM = await _categoryApiClient.GetFeaturedCategory(
                    SystemConstants.LanguageSettings.DefaultLanguageId,
                    SystemConstants.CategorySettings.NumberOfFeaturedCategory
                );
                allRouteData.Add("categoryId", "0");
            }
            else
            {
                categoryVM = await _categoryApiClient.GetActiveCategory(SystemConstants.LanguageSettings.DefaultLanguageId);
                allRouteData = new Dictionary<string, string>()
                {
                    { "pageIndex", "1" },
                    { "pageSize", SystemConstants.ProductSettings.pageSizePaging.ToString()},
                    { "categoryId", "0"}
                };
            }

            return View(new CategoryVM()
            {
                BaseCategoryViewModels = categoryVM,
                AllRouteData = allRouteData,
                IsFeatured = isFeatured
            });
        }
    }
}