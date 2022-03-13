using ECommerce.Models.Entities;
using ECommerce.Models.Enums;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DataAccess.EF
{
    public class ECommerceDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Fluent API

            // Entities Configuaration
            modelBuilder.Entity<Category>()
                .Property(x => x.Status);
            modelBuilder.Entity<CategoryTranslation>()
                .Property(x => x.LanguageId).IsUnicode(false);
            modelBuilder.Entity<Language>()
                .Property(x => x.Id).IsUnicode(false);
            modelBuilder.Entity<Order>()
                .Property(x => x.ShipEmail).IsUnicode(false);
            modelBuilder.Entity<Contact>()
                .Property(x => x.Email).IsUnicode(false);
            modelBuilder.Entity<Product>()
                .Property(x => x.Stock).HasDefaultValue(0);
            modelBuilder.Entity<Product>()
                .Property(x => x.ViewCount).HasDefaultValue(0);
            modelBuilder.Entity<ProductTranslation>()
                 .Property(x => x.Id).IsUnicode(false);
            modelBuilder.Entity<ProductInCategory>()
                .HasKey(t => new { t.CategoryId, t.ProductId });
            modelBuilder.Entity<ProductInCategory>()
                .HasOne(t => t.Product)
                .WithMany(pc => pc.ProductInCategories)
                .HasForeignKey(pc => pc.ProductId);
            modelBuilder.Entity<ProductInCategory>()
                .HasOne(t => t.Category)
                .WithMany(pc => pc.ProductInCategories)
                .HasForeignKey(pc => pc.CategoryId);
            modelBuilder.Entity<OrderDetail>()
                .HasKey(t => new { t.OrderId, t.ProductId });
            modelBuilder.Entity<OrderDetail>()
                .HasOne(t => t.Order)
                .WithMany(pc => pc.OrderDetails)
                .HasForeignKey(pc => pc.ProductId);
            modelBuilder.Entity<OrderDetail>()
                .HasOne(t => t.Product)
                .WithMany(pc => pc.OrderDetails)
                .HasForeignKey(pc => pc.ProductId);
            modelBuilder.Entity<ProductImage>()
                .HasKey(t => new { t.ProductId, t.ImageId });
            modelBuilder.Entity<UserImage>()
                .HasKey(t => new { t.userId, t.ImageId });

            // Identity configuration
            modelBuilder.Entity<User>()
                .ToTable("Users");
            modelBuilder.Entity<Role>()
                .ToTable("Roles");
            modelBuilder.Entity<IdentityUserClaim<Guid>>()
                .ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>()
                .ToTable("UserRoles")
                .HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>()
                .ToTable("UserLogins")
                .HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>()
                .ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>()
                .ToTable("UserTokens");

            #endregion Fluent API

            // seed data
            modelBuilder.Seed();
        }

        #region Add Tables

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<AppConfig> AppConfigs { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }

        public DbSet<ProductInCategory> ProductInCategories { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<ProductTranslation> ProductTranslations { get; set; }

        public DbSet<Promotion> Promotions { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<UserImage> UserImages { get; set; }

        public DbSet<ProductReview> ProductReviews { get; set; }

        public DbSet<Slide> Slides { get; set; }

        #endregion Add Tables
    }
}