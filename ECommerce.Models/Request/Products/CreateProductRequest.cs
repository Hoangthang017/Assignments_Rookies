using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Request.Products
{
    public class CreateProductRequest : BaseProductRequest
    {
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Original price is required")]
        public decimal OriginalPrice { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "LanguageId is required")]
        [MaxLength(5, ErrorMessage = "Max length of languageId is 5 characters")]
        public string LanguageId { set; get; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { set; get; }
    }
}