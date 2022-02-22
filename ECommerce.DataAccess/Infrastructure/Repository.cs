using ECommerce.DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.DataAccess.Infrastructure
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

        #region Implementation

        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task DeleteMulti(Expression<Func<T, bool>> where)
        {
            IAsyncEnumerable<T> objects = dbSet.Where<T>(where).AsAsyncEnumerable();
            await foreach (T obj in objects)
                dbSet.Remove(obj);
        }

        public async Task<T> GetSingleById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<int> Count(Expression<Func<T, bool>> where)
        {
            return await dbSet.CountAsync(where);
        }

        public IQueryable<T> GetAll(string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = _context.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.AsQueryable();
            }

            return _context.Set<T>().AsQueryable();
        }

        public async Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            return await GetAll(includes).FirstOrDefaultAsync(expression);
        }

        public IQueryable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = _context.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.Where<T>(predicate).AsQueryable<T>();
            }

            return _context.Set<T>().Where<T>(predicate).AsQueryable<T>();
        }

        public async Task<bool> CheckContains(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync<T>(predicate) > 0;
        }

        #endregion Implementation
    }
}