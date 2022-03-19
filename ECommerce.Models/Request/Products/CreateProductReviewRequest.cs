using ECommerce.Models.Enums;

namespace ECommerce.Models.Request.Products
{
    public class CreateProductReviewRequest
    {
        public Rating Rating { get; set; }

        public string Comment { get; set; }
    }
}