using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AspNetMVCServerSideDatatableExample.EntityFramework
{
    public interface IRepository<T>
    {
        void Insert(T obj);
        void Update(T obj);
        void Delete(T obj);
        void Delete(object id);
        void Save();

        Task InsertAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(T obj);
        Task DeleteAsync(object id);
        Task SaveAsync();

        IQueryable<T> Including(params Expression<Func<T, object>>[] includeProperties);

        IEnumerable<T> List(
        Expression<Func<T, bool>> filter = null,
        string includeProperties = "",
        int? skip = null,
        int? take = null,
        bool asNoTracking = false);

        Task<IEnumerable<T>> ListAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null,
            bool asNoTracking = false);

        T Find(
                    Expression<Func<T, bool>> filter = null,
                    string includeProperties = "");

        Task<T> FindAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = "");

        T GetById(object id);

        Task<T> GetByIdAsync(object id);

        int Count(Expression<Func<T, bool>> filter = null);

        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);

        bool Exists(Expression<Func<T, bool>> filter = null);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter = null);
    }
}
