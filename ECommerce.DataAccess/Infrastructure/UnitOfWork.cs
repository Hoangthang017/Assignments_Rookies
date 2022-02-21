using ECommerce.DataAccess.EF;
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
            Products = new ProductRepository(context);
        }

        public IProductRepository Products { get; private set; }

        async Task IUnitOfWork.Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}