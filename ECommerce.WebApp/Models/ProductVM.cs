using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Products;

namespace ECommerce.WebApp.Models
{
    public class ProductVM
    {
        public ProductViewModel ProductViewModel { get; set; }

        public List<ProductViewModel> RelatedProducts { get; set; }

        public List<ProductReviewViewModel> ProductReviewViewModels { get; set; }

        public double GetTotalRating()
        {
            if (ProductReviewViewModels.Count == 0) return 0;
            var totalRating = 0.0;
            foreach (var productReview in ProductReviewViewModels)
            {
                totalRating += (int)productReview.Rating;
            }
            return totalRating / ProductReviewViewModels.Count;
        }
    }
}