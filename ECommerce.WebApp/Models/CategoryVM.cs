using ECommerce.Models.ViewModels.Categories;

namespace ECommerce.WebApp.Models
{
    public class CategoryVM
    {
        public List<BaseCategoryViewModel> BaseCategoryViewModels { get; set; }

        public Dictionary<string, string> AllRouteData { get; set; }

        public bool IsFeatured { get; set; }
    }
}