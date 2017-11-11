using System;
using System.Linq;
using System.Linq.Expressions;


namespace Nop.Core
{
    public class Orderable<T>
    {
        private IQueryable<T> _queryable;
        public Orderable(IQueryable<T> enumerable)
        {
            _queryable = enumerable;
        }
        public IQueryable<T> Queryable
        {
            get { return _queryable; }
        }

        /// <summary>
        /// 递增
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public Orderable<T> Asc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _queryable = _queryable.OrderBy(keySelector);
            return this;
        }

        /// <summary>
        /// 递减
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public Orderable<T> Desc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _queryable = _queryable.OrderByDescending(keySelector);
            return this;
        }

        /// <summary>
        /// 然后递增
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public Orderable<T> ThenAsc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _queryable = (_queryable as IOrderedQueryable<T>)
                .ThenBy(keySelector);
            return this;
        }

        /// <summary>
        /// 然后递减
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public Orderable<T> ThenDesc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            _queryable = (_queryable as IOrderedQueryable<T>)
                 .ThenByDescending(keySelector);
            return this;
        }
    }
}
