using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Nop.Core.Caching
{
    public class ScdMemoryCacheManager : IScdCacheManager
    {
        public TResult Get<TResult>(ScdCacheKey key)
        {
            return (TResult)Cache[key.ToString()];
        }

        public void Set(ScdCacheKey key, object data, double cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key.ToString(), data), policy);
        }

        public bool IsSet(ScdCacheKey key)
        {
            return (Cache.Contains(key.ToString()));
        }

        public void Remove(ScdCacheKey key)
        {
            Cache.Remove(key.ToString());
        }


        /// <summary>
        /// Cache object
        /// </summary>
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }


        public void AddCache(ScdCacheKey key, object value, string dependencyFile)
        {
            if (!IsSet(key))
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                List<string> filePath = new List<string>();
                filePath.Add(dependencyFile);
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePath));
                Cache.Set(key.ToString(), value, policy);
            }
        }
    }
}
