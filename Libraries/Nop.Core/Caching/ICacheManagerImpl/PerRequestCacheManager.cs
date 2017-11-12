using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nop.Core.Caching
{
    public class PerRequestCacheManager : ICacheManager
    {
        private readonly HttpContextBase _context;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        public PerRequestCacheManager(HttpContextBase context)
        {
            this._context = context;
        }

        /// <summary>
        /// Creates a new instance of the ScdRequestCache class
        /// </summary>
        protected virtual IDictionary GetItems()
        {
            if (_context != null)
                return _context.Items;

            return null;
        }

        public TResult Get<TResult>(ScdCacheKey key)
        {
            var items = GetItems();
            if (items == null)
                return default(TResult);

            return (TResult)items[key.ToString()];
        }

        public void Set(ScdCacheKey key, object data, double cacheTime)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
            {
                if (items.Contains(key.ToString()))
                    items[key.ToString()] = data;
                else
                    items.Add(key.ToString(), data);
            }
        }

        public bool IsSet(ScdCacheKey key)
        {
            var items = GetItems();
            if (items == null)
                return false;

            return (items[key.ToString()] != null);
        }

        public void Remove(ScdCacheKey key)
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Remove(key.ToString());
        }
    }
}
