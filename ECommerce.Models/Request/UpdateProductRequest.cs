namespace ECommerce.Models.Request
{
    public class UpdateProductRequest
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public string Image { get; set; }

        public string MoreImages { get; set; }

        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        public int? Warranty { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public bool? HomeFlag { get; set; }

        public bool? HotFlag { get; set; }

        public string MeteKeyword { get; set; }

        public string MetaDescription { get; set; }

        public bool Status { get; set; }
    }
}