using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure.Common;
using ECommerce.DataAccess.Repository.ProductRepo;
using ECommerce.DataAccess.Repository.UserRepo;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ECommerce.DataAccess.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceDbContext _context;

        public UnitOfWork(
            ECommerceDbContext context,
            IStorageService storageService,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration)
        {
            _context = context;

            Product = new ProductRepository(context, storageService, mapper);

            ProductImage = new ProductImageRepository(context, storageService, mapper);

            ProductInCategory = new ProductInCategoryRepository(context);

            ProductReview = new ProductReviewRepository(context);

            ProductTranslation = new ProductTranslationRepository(context);

            Category = new CategoryRepository(context);

            CategoryTranslation = new CategoryTranslationRepository(context);

            User = new UserRepository(
                context,
                userManager,
                signInManager,
                roleManager,
                configuration);
        }

        #region Product

        public IProductRepository Product { get; private set; }

        public IProductImageRepository ProductImage { get; private set; }

        public IProductInCategoryRepository ProductInCategory { get; private set; }

        public IProductReviewRepository ProductReview { get; private set; }

        public IProductTranslationRepository ProductTranslation { get; private set; }

        #endregion Product

        public ICategoryRepository Category { get; private set; }

        public ICategoryTranslationRepository CategoryTranslation { get; private set; }

        public IUserRepository User { get; private set; }

        // save method
        async Task<bool> IUnitOfWork.Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}