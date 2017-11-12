using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nop.Services
{
    public class BaseService<TEntity, TKey> : IBaseService<TEntity, TKey> where TEntity : class
    {
        protected IRepository<TEntity> _repository;
        public BaseService()
        {
            _repository = EngineContext.Current.Resolve<IRepository<TEntity>>();
        }

        public virtual void Insert(TEntity entity)
        {
            _repository.Insert(entity);
        }
        public virtual void Update(TEntity entity)
        {
            _repository.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _repository.Delete(entity);
        }
        
        public virtual IQueryable<TEntity> Table
        {
            get
            {
                return _repository.Table;
            }
        }
        public virtual TEntity FindBy(TKey id)
        {
            return _repository.GetById(id);
        }
        public virtual TEntity FindBy(Expression<Func<TEntity, bool>> expression)
        {
            return Table.Where(expression).SingleOrDefault<TEntity>();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> expression)
        {
            return Table.Where(expression).Count();
        }

        public virtual IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> expression)
        {
            return FetchWithQueryable(expression);
        }

        public virtual IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> expression, Action<Orderable<TEntity>> order)
        {
            return FetchWithQueryable(expression, order);
        }

        public virtual IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> expression, Action<Orderable<TEntity>> order, int skip, int count)
        {
            return FetchWithQueryable(expression, order, skip, count);
        }


        public virtual IQueryable<TEntity> FetchWithQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return Table.Where(predicate);
        }

        public virtual IQueryable<TEntity> FetchWithQueryable(Expression<Func<TEntity, bool>> predicate, Action<Orderable<TEntity>> order)
        {
            var orderable = new Orderable<TEntity>(FetchWithQueryable(predicate));
            order(orderable);
            return orderable.Queryable;
        }

        public virtual IQueryable<TEntity> FetchWithQueryable(Expression<Func<TEntity, bool>> predicate, Action<Orderable<TEntity>> order, int skip,
                                           int count)
        {
            return FetchWithQueryable(predicate, order).Skip(skip).Take(count);
        }

 
    }
}
