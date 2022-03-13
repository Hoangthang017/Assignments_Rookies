using ECommerce.Models.ViewModels.Categories;
using ECommerce.Models.ViewModels.Products;
using ECommerce.Models.ViewModels.Slides;

namespace ECommerce.WebApp.Models
{
    public class HomeViewModel
    {
        public List<SlideViewModel> Slides { get; set; }

        public List<ProductViewModel> FeaturedProducts { get; set; }

        public List<BaseCategoryViewModel> FeaturedCategories { get; set; }
    }
}