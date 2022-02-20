using System.Linq.Expressions;

namespace ECommerce.DataAccess.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        // Marks an entity as new
        Task Add(T entity);

        // Marks an entity as modified
        void Update(T entity);

        // Marks an entity to be removed
        void Delete(T entity);

        //Delete multi records
        Task DeleteMulti(Expression<Func<T, bool>> where);

        // Get an entity by int id
        Task<T> GetSingleById(int id);

        Task<T> GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        IQueryable<T> GetAll(string[] includes = null);

        IQueryable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        IQueryable<T> GetMultiPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null);

        Task<int> Count(Expression<Func<T, bool>> where);

        Task<bool> CheckContains(Expression<Func<T, bool>> predicate);
    }
}