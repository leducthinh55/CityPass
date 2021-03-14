using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebAPI.Utils
{
    public static class GenericSorter
    {
        public static IQueryable<T> Sort<T>(IQueryable<T> source, string sortBy, int sortDir = -1)
        {
            var param = Expression.Parameter(typeof(T), "item");

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            if (sortDir == -1)
            {
                return source.OrderByDescending<T, object>(sortExpression);
            }
            return source.OrderBy<T, object>(sortExpression);
        }

        public static IOrderedQueryable<T> Sort<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keySelector, int sortDir)
        {
            if (sortDir == -1)
            {
                return source.OrderByDescending(keySelector);
            }
            return source.OrderBy(keySelector);
        }
    }
}
