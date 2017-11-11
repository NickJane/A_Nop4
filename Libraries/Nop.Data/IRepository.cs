using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nop.Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(T entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<T> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

        /// <summary>
        /// 查询单个记录
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        T FindBy(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 查询一个集合
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<T> Fetch(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 查询一个集合，可排序
        /// </summary>
        /// <param name="expression">查询条件-lambda表达式树</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        IQueryable<T> Fetch(Expression<Func<T, bool>> expression, Action<Orderable<T>> order);
    }
}
