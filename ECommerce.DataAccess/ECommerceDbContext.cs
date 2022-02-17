using ECommerce.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Các Fluent API
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasOne(x => x.Product).WithMany(x => x.ProductImages).HasForeignKey(x => x.ProductId);
            });

            modelBuilder.Entity<ProductTranslation>(entity =>
            {
                entity.HasOne(x => x.Language).WithMany(x => x.ProductTranslations).HasForeignKey(x => x.LanguageId);

                entity.HasOne(x => x.Product).WithMany(x => x.ProductTranslations).HasForeignKey(x => x.ProductId);
            });

            modelBuilder.Entity<CategoryTranslation>(entity =>
            {
                entity.HasOne(x => x.Language).WithMany(x => x.CategoryTranslations).HasForeignKey(x => x.LanguageId);

                entity.HasOne(x => x.Category).WithMany(x => x.CategoryTranslations).HasForeignKey(x => x.CategoryId);
            });

            modelBuilder.Entity<ProductInCategory>(entity =>
            {
                entity.HasKey(x => new { x.CategoryId, x.ProductId });

                entity.HasOne(x => x.Product).WithMany(x => x.ProductInCategories)
                .HasForeignKey(x => x.ProductId);

                entity.HasOne(x => x.Category).WithMany(x => x.ProductInCategories)
                  .HasForeignKey(x => x.CategoryId);
            });
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<ProductInCategory> ProductInCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Language> Languages { get; set; }
    }
}