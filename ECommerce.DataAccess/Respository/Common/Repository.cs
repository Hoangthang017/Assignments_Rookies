using ECommerce.DataAccess.EF;
using ECommerce.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Respository.Common
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ECommerceDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(ECommerceDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public async Task<List<T>> GetAll()
        {
            IQueryable<T> query = dbSet;
            return await query.ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            T entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new ECommerceException($"Cannot find a product with id: {id}");
            }
            return entity;
        }

        public async Task<bool> Remove(object id)
        {
            T entity = await GetById(id);
            _context.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}