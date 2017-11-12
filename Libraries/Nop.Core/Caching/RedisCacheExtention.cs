using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nop.Core.Caching
{
    /// <summary>
    /// 功能：redis缓存的扩展方法
    /// 时间：2017/3/8
    /// </summary>
    public class RedisCacheExtention
    {
        private readonly RedisCacheDatabaseProvider _cacheProvider;
        private readonly IDatabase _db;

        public RedisCacheExtention()
        {
            _cacheProvider = new RedisCacheDatabaseProvider();
            _db = _cacheProvider.GetDatabase();
        }

        #region Utilities

        protected virtual byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(jsonString);
        }
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 获取分布式锁
        /// 参考：http://stackoverflow.com/questions/25127172/stackexchange-redis-locktake-lockrelease-usage
        /// </summary>
        /// <param name="lockName"></param>
        /// <param name="lockToken"></param>
        /// <returns></returns>
        private bool AcquireLock(string lockName, string lockToken)
        {
            var totalTime = TimeSpan.Zero;
            var maxTime = TimeSpan.FromSeconds(3);
            var expiration = TimeSpan.FromSeconds(3);
            var sleepTime = TimeSpan.FromMilliseconds(50);
            var lockAchieved = false;

            while (!lockAchieved && totalTime < maxTime)
            {
                lockAchieved = _db.LockTake(lockName, lockToken, expiration);
                if (lockAchieved)
                {
                    return true;
                }
                Thread.Sleep(sleepTime);
                totalTime += sleepTime;
            }
            return false;
        }

        #endregion


        public string LockStringGet(ScdCacheKey cacheKey)
        {
            string lockName = "lockKey_" + cacheKey.ToString();
            string lockToken = Guid.NewGuid().ToString();

            if (AcquireLock(lockName, lockToken))
            {
                try
                {
                    var rValue = _db.StringGet(cacheKey.ToString());
                    if (!rValue.HasValue)
                    {
                        return null;
                    }
                    var result = Deserialize<string>(rValue);
                    return result;
                }
                finally
                {
                    _db.LockRelease(lockName, lockToken);
                }
            }
            else
            {
                return null;
            }
        }

        public void LockStringSet(ScdCacheKey cacheKey, string vlaue)
        {
            string lockName = "lockKey_" + cacheKey.ToString();
            string lockToken = Guid.NewGuid().ToString();

            if (AcquireLock(lockName, lockToken))
            {
                try
                {
                    var entryBytes = Serialize(vlaue);
                    var expiresIn = TimeSpan.FromMinutes(10);

                    _db.StringSet(cacheKey.ToString(), entryBytes, expiresIn);
                }

                finally
                {
                    _db.LockRelease(lockName, lockToken);
                }
            }
        }

    }
}
