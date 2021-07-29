using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Acc.HelpersAndUtilities.Generic
{
    public interface IGenericRepository<T>
         where T : class
    {
        Task<IList<T>> AddRangeAsyn(IList<T> t);
        Task<T> AddAsyn(T t);
        Task<IList<T>> FindAllAsyn();
        Task<IQueryable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<int> DeleteAsyn(T entity);
        Task<T> UpdateAsyn(T t, object key);
        Task<T> FirstOrDefaultAsyn(Expression<Func<T, bool>> predicate);

        Task<IQueryable<T>> FindByQueryableAsync(Expression<Func<T, bool>> predicate = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                          bool disableTracking = true,
                                          int? skip = null, int? take = null);

        Task<T> FindByQueryableSingleAsync(Expression<Func<T, bool>> predicate = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                          bool disableTracking = true,
                                          int? skip = null, int? take = null);
    }
}
