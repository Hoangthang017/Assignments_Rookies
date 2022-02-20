using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Repository.CommonRepo;
using ECommerce.DataAccess.Repository.OrderRepo;
using ECommerce.DataAccess.Repository.ProductRepo;
using ECommerce.DataAccess.Respository.Common;

namespace ECommerce.DataAccess.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceDbContext _context;

        public UnitOfWork(ECommerceDbContext context)
        {
            _context = context;
            Footers = new FooterRepository(context);
            Products = new ProductRepository(context);
            Posts = new PostRepository(context);
        }

        public IFooterRepository Footers { get; private set; }

        public IProductRepository Products { get; private set; }

        public IPostRepository Posts { get; private set; }

        async Task<bool> IUnitOfWork.Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}