using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace AspNetMVCServerSideDatatableExample.EntityFramework
{
    public class Repository<T> : RepositoryBase, IRepository<T> where T : class
    {

        private DbSet<T> _objectset;

        public Repository()
        {
            _objectset = context.Set<T>();
        }

        protected virtual IQueryable<T> GetQueryable(
        Expression<Func<T, bool>> filter = null,
        string includeProperties = "",
        int? skip = null,
        int? take = null,
        bool asNoTracking = false)
        {
            IQueryable<T> query = _objectset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public int Count(Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).Count();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).CountAsync();
        }

        public T Find(
       Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            return GetQueryable(filter, includeProperties).FirstOrDefault();
        }

        public async Task<T> FindAsync(
       Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            return await GetQueryable(filter, includeProperties).FirstOrDefaultAsync();
        }

        public T GetById(object id)
        {
            return _objectset.Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _objectset.FindAsync(id);
        }

        public IQueryable<T> Including(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = _objectset;
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public void Insert(T obj)
        {
            _objectset.Add(obj);
            Save();
        }

        public void Update(T obj)
        {
            //var entry = this.Context.Entry(entityToUpdate);
            //var key = this.GetPrimaryKey(entry);

            //if (entry.State == EntityState.Detached)
            //{
            //    var currentEntry = GetById(key);
            //    if (currentEntry != null)
            //    {
            //        var attachedEntry = Context.Entry(currentEntry);
            //        attachedEntry.CurrentValues.SetValues(entityToUpdate);
            //    }
            //    else
            //    {
            //        DbSet.Attach(entityToUpdate);
            //        Context.Entry(entityToUpdate).State = EntityState.Modified;
            //    }
            //}

            //var entry = Context.Entry(entity);
            //if (entry.State == EntityState.Detached)
            //    Set.Attach(entity);
            //entry.State = EntityState.Modified;

            context.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public void Delete(T obj)
        {
            _objectset.Remove(obj);
            Save();
        }

        public void Delete(object id)
        {
            T entity = _objectset.Find(id);
            _objectset.Remove(entity);
            Save();
        }

        public void Save()
        {
            //bool returnValue = true;
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    //returnValue = false;
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
            //context.SaveChanges();
            //return returnValue;
        }

        public IEnumerable<T> List(Expression<Func<T, bool>> filter = null, string includeProperties = "", int? skip = null, int? take = null, bool asNoTracking = false)
        {
            return GetQueryable(filter, includeProperties, skip, take, asNoTracking).ToList();
        }

        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "", int? skip = null, int? take = null, bool asNoTracking = false)
        {
            return await GetQueryable(filter, includeProperties, skip, take, asNoTracking).ToListAsync();
        }

        public async Task InsertAsync(T obj)
        {
            _objectset.Add(obj);
            await SaveAsync();
        }

        public async Task UpdateAsync(T obj)
        {
            context.Entry(obj).State = EntityState.Modified;
            await SaveAsync();
        }

        public async Task DeleteAsync(T obj)
        {
            _objectset.Remove(obj);
            await SaveAsync();
        }

        public async Task DeleteAsync(object id)
        {
            T entity = await _objectset.FindAsync(id);
            _objectset.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            //bool returnValue = true;
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    await context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    //returnValue = false;
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
            //await context.SaveChangesAsync();
            //return returnValue;
        }

        public bool Exists(Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).Any();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter = null)
        {
            return await GetQueryable(filter).AnyAsync();
        }

        public IEnumerable<T> GetSql(string sql)
        {
            return _objectset.SqlQuery(sql);
        }
    }
}