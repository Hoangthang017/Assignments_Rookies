using Microsoft.AspNetCore.Http;

namespace ECommerce.Models.ViewModels.Products
{
    public class ProductViewModel
    {
        // product
        public int Id { get; set; }

        public decimal Price { get; set; }

        public decimal OriginalPrice { get; set; }

        public int Stock { get; set; }

        public int ViewCount { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        // product translation
        public string Name { set; get; }

        public string Description { set; get; }

        public string Details { set; get; }

        public string SeoDescription { set; get; }

        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }

        public string LanguageId { set; get; }

        // category
        public int categoryId { get; set; }

        public string CategoryName { get; set; }

        // image
        public List<string> imagePaths { get; set; }
    }
}