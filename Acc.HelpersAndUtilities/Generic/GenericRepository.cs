using Acc.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Acc.HelpersAndUtilities.Generic
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        private readonly SqlServerContext _sqlServerContext;
        internal DbSet<T> dbSet;

        public GenericRepository(SqlServerContext sqlServerContext)
        {
            this._sqlServerContext = sqlServerContext;
            this.dbSet = sqlServerContext.Set<T>();
        }

        public virtual async Task<IList<T>> AddRangeAsyn(IList<T> t)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _sqlServerContext.Set<T>().AddRangeAsync(t);
            await _sqlServerContext.SaveChangesAsync();
            return t;
        }
        public virtual async Task<T> AddAsyn(T t)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            _sqlServerContext.Set<T>().Add(t);
            await _sqlServerContext.SaveChangesAsync();
            return t;
        }

        public async Task<IList<T>> FindAllAsyn()
             => await _sqlServerContext.Set<T>().ToListAsync();

        public virtual async Task<IQueryable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                var query = await _sqlServerContext.Set<T>().Where(predicate).ToListAsync();
                return query.AsQueryable();
            }
            else
            {
                var query = await _sqlServerContext.Set<T>().ToListAsync();
                return query.AsQueryable();
            }

        }

        public virtual async Task<int> DeleteAsyn(T entity)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            _sqlServerContext.Set<T>().Remove(entity);
            return await _sqlServerContext.SaveChangesAsync();
        }
        public virtual async Task<T> UpdateAsyn(T t, object key)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;

            if (t == null)
                return null;
            T exist = await _sqlServerContext.Set<T>().FindAsync(key);
            if (exist != null)
            {
                _sqlServerContext.Entry(exist).CurrentValues.SetValues(t);
                await _sqlServerContext.SaveChangesAsync();
            }
            return exist;
        }

        public async Task<T> FirstOrDefaultAsyn(Expression<Func<T, bool>> predicate)
            => predicate == null ? await _sqlServerContext.Set<T>().FirstOrDefaultAsync() : await _sqlServerContext.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task<IQueryable<T>> FindByQueryableAsync(Expression<Func<T, bool>> predicate = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                          bool disableTracking = true,
                                          int? skip = null, int? take = null)
        {
            IQueryable<T> query = dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (skip.HasValue)
            {
                query = query.Skip((skip.Value - 1) * take.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await Task.FromResult(query);
        }

        public async Task<T> FindByQueryableSingleAsync(Expression<Func<T, bool>> predicate = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                          bool disableTracking = true,
                                          int? skip = null, int? take = null)
        {
            IQueryable<T> query = dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (skip.HasValue)
            {
                query = query.Skip((skip.Value - 1) * take.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
