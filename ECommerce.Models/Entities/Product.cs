using ECommerce.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public decimal OriginalPrice { get; set; }

    [Required]
    public int Stock { get; set; }

    [Required]
    public int ViewCount { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public IEnumerable<Cart> Carts { get; set; }

    public IEnumerable<OrderDetail> OrderDetails { get; set; }

    public IEnumerable<ProductInCategory> ProductInCategories { get; set; }

    public IEnumerable<ProductTranslation> ProductTranslations { get; set; }

    public IEnumerable<ProductImage> ProductImages { get; set; }

    public IEnumerable<ProductReview> ProductReviews { get; set; }
}