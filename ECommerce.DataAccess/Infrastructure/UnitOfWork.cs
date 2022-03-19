using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure.Common;
using ECommerce.DataAccess.Repository.ImageRepo;
using ECommerce.DataAccess.Repository.OrderRepo;
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

            Category = new CategoryRepository(context, mapper);

            Image = new ImageRepository(context, storageService);

            User = new UserRepository(
                context,
                userManager,
                signInManager,
                roleManager,
                storageService,
                mapper,
                configuration);

            Order = new OrderRepository(context);
        }

        public IProductRepository Product { get; private set; }

        public ICategoryRepository Category { get; private set; }

        public IUserRepository User { get; private set; }

        public IImageRepository Image { get; private set; }

        public IOrderRepository Order { get; private set; }

        // save method
        async Task<bool> IUnitOfWork.Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}