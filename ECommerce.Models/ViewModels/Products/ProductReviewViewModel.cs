using ECommerce.Models.Enums;

namespace ECommerce.Models.ViewModels.Products
{
    public class ProductReviewViewModel
    {
        public int Id { get; set; }

        public DateTime ReviewDate { get; set; }

        public Rating Rating { get; set; }

        public string Comment { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerUsername { get; set; }

        public string customerAvatar { get; set; }
    }
}