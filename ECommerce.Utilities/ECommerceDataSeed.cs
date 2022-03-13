using ECommerce.Models.Entities;
using ECommerce.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Utilities
{
    public static class ECommerceDataSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
               new AppConfig() { Key = "HomeTitle", Value = "This is home page of ECommerce" },
               new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of eShopSolution" },
               new AppConfig() { Key = "HomeDescription", Value = "This is description of eShopSolution" }
            );
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi-VN", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en-US", Name = "English", IsDefault = false });

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 1,
                    Status = Status.Active,
                },
                 new Category()
                 {
                     Id = 2,
                     IsShowOnHome = true,
                     ParentId = null,
                     SortOrder = 2,
                     Status = Status.Active
                 });

            modelBuilder.Entity<CategoryTranslation>().HasData(
                  new CategoryTranslation() { Id = 1, CategoryId = 1, Name = "Áo nam", LanguageId = "vi-VN", SeoAlias = "ao-nam", SeoDescription = "Sản phẩm áo thời trang nam", SeoTitle = "Sản phẩm áo thời trang nam" },
                  new CategoryTranslation() { Id = 2, CategoryId = 1, Name = "Men Shirt", LanguageId = "en-US", SeoAlias = "men-shirt", SeoDescription = "The shirt products for men", SeoTitle = "The shirt products for men" },
                  new CategoryTranslation() { Id = 3, CategoryId = 2, Name = "Áo nữ", LanguageId = "vi-VN", SeoAlias = "ao-nu", SeoDescription = "Sản phẩm áo thời trang nữ", SeoTitle = "Sản phẩm áo thời trang women" },
                  new CategoryTranslation() { Id = 4, CategoryId = 2, Name = "Women Shirt", LanguageId = "en-US", SeoAlias = "women-shirt", SeoDescription = "The shirt products for women", SeoTitle = "The shirt products for women" }
                    );

            modelBuilder.Entity<Product>().HasData(
           new Product()
           {
               Id = 1,
               CreatedDate = DateTime.Now,
               OriginalPrice = 100000,
               Price = 200000,
               Stock = 0,
               ViewCount = 0,
           });
            modelBuilder.Entity<ProductTranslation>().HasData(
                 new ProductTranslation()
                 {
                     Id = 1,
                     ProductId = 1,
                     Name = "Áo sơ mi nam trắng Việt Tiến",
                     LanguageId = "vi-VN",
                     SeoAlias = "ao-so-mi-nam-trang-viet-tien",
                     SeoDescription = "Áo sơ mi nam trắng Việt Tiến",
                     SeoTitle = "Áo sơ mi nam trắng Việt Tiến",
                     Details = "Áo sơ mi nam trắng Việt Tiến",
                     Description = "Áo sơ mi nam trắng Việt Tiến"
                 },
                    new ProductTranslation()
                    {
                        Id = 2,
                        ProductId = 1,
                        Name = "Viet Tien Men T-Shirt",
                        LanguageId = "en-US",
                        SeoAlias = "viet-tien-men-t-shirt",
                        SeoDescription = "Viet Tien Men T-Shirt",
                        SeoTitle = "Viet Tien Men T-Shirt",
                        Details = "Viet Tien Men T-Shirt",
                        Description = "Viet Tien Men T-Shirt"
                    });
            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { ProductId = 1, CategoryId = 1 }
                );

            // seeding data for identity
            var ROLE_ADMIN_ID = new Guid("F972B64F-6780-4657-9AE2-4BB4BA262024");
            var ROLE_CUSTOMER_ID = new Guid("16A33B7F-8765-4E91-8C8D-C8A2C979A9CD");
            var ADMIN_ID = new Guid("644F5CAA-4B11-44A0-AF41-0FD7A8DE18EE");
            var CUSTOMER_ID = new Guid("DD9EFC6A-1CA0-4E0B-9362-5FB185558A33");
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = ROLE_ADMIN_ID,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            },
            new Role
            {
                Id = ROLE_CUSTOMER_ID,
                Name = "customer",
                NormalizedName = "customer",
                Description = "customer role"
            });

            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "thangnh1394@gmail.com",
                NormalizedEmail = "thangnh1394@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "123456"),
                SecurityStamp = string.Empty,
                FirstName = "Thang",
                LastName = "Nguyen",
                DateOfBirth = new DateTime(2000, 11, 13)
            },
            new User
            {
                Id = CUSTOMER_ID,
                UserName = "customer",
                NormalizedUserName = "customer",
                Email = "thangnh1394@gmail.com",
                NormalizedEmail = "thangnh1394@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "123456"),
                SecurityStamp = string.Empty,
                FirstName = "Thang",
                LastName = "Nguyen",
                DateOfBirth = new DateTime(2000, 11, 13)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = ROLE_ADMIN_ID,
                UserId = ADMIN_ID
            },
            new IdentityUserRole<Guid>
            {
                RoleId = ROLE_CUSTOMER_ID,
                UserId = CUSTOMER_ID
            });

            modelBuilder.Entity<Image>().HasData(
              new Image()
              {
                  Id = 1,
                  DateCreated = DateTime.Now,
                  Caption = "We serve Fresh Vegestables",
                  FileSize = 99999,
                  ImagePath = SystemConstants.AppSettings.BackendApiAddress + "/user-content/slide/bg_1.jpg",
                  SortOrder = 1,
              },
                new Image()
                {
                    Id = 2,
                    DateCreated = DateTime.Now,
                    Caption = "100% Fresh &amp; Organic Foods",
                    FileSize = 99999,
                    ImagePath = SystemConstants.AppSettings.BackendApiAddress + "/user-content/slide/bg_2.jpg",
                    SortOrder = 1,
                }
           );

            modelBuilder.Entity<Slide>().HasData(
              new Slide() { Id = 1, Description = "We deliver organic vegetables &amp; fruits", ImageId = 1 },
              new Slide() { Id = 2, Description = "We deliver organic vegetables &amp; fruits", ImageId = 2 }
           );
        }
    }
}