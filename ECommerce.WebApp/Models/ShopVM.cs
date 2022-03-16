using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Products;

namespace ECommerce.WebApp.Models
{
    public class ShopVM
    {
        public PageResult<ProductViewModel> ProductViewModels { get; set; }
    }
}