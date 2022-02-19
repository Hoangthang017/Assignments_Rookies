using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Respository.Common
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter);

        Task<List<T>> GetAll();

        Task<T> GetById(object id);

        Task<bool> Remove(object id);
    }
}