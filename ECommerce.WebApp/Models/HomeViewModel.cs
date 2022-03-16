using ECommerce.Models.ViewModels.Categories;
using ECommerce.Models.ViewModels.Products;
using ECommerce.Models.ViewModels.Slides;

namespace ECommerce.WebApp.Models
{
    public class HomeViewModel
    {
        public List<ProductViewModel> FeaturedProducts { get; set; }

        public bool isFeatured { get; set; }
    }
}