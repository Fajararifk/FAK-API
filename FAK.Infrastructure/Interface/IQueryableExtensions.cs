using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Infrastructure.Interface
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> EagerLoadInclude<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
                query = includes.Aggregate(query,
                    (current, include) => current.Include(include));

            return query;
        }

        public static IQueryable<T> EagerLoadWhere<T>(this IQueryable<T> query, Expression<Func<T, bool>> wheres) where T : class
        {
            if (wheres != null)
                query = query.Where(wheres);

            return query;
        }

        public static bool IsOrdered<T>(this IQueryable<T> queryable)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException("queryable");
            }

            return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
        }

        public static IQueryable<T> SmartOrderBy<T, TKey>(this IQueryable<T> queryable, Expression<Func<T, TKey>> keySelector)
        {
            if (queryable.IsOrdered())
            {
                var orderedQuery = queryable as IOrderedQueryable<T>;
                return orderedQuery.ThenBy(keySelector);
            }
            else
            {
                return queryable.OrderBy(keySelector);
            }
        }

        public static IQueryable<T> SmartOrderByDescending<T, TKey>(this IQueryable<T> queryable, Expression<Func<T, TKey>> keySelector)
        {
            if (queryable.IsOrdered())
            {
                var orderedQuery = queryable as IOrderedQueryable<T>;
                return orderedQuery.ThenByDescending(keySelector);
            }
            else
            {
                return queryable.OrderByDescending(keySelector);
            }
        }
    }
}
