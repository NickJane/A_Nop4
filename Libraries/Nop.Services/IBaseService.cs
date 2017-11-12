using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nop.Services
{
    public interface IBaseService<TEntity, Tkey> where TEntity : class
    {
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        TEntity FindBy(Tkey Id);
        TEntity FindBy(Expression<Func<TEntity, bool>> expression);
        int Count(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> Table { get; }
        IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> expression, Action<Orderable<TEntity>> order);
        IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> expression, Action<Orderable<TEntity>> order, int skip, int count);
    }
}
