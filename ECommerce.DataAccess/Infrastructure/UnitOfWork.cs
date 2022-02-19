using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Respository.Common;

namespace ECommerce.DataAccess.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceDbContext _context;

        public UnitOfWork(ECommerceDbContext context)
        {
            _context = context;
        }

        async Task IUnitOfWork.Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}