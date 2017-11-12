using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Text;

namespace Nop.Core.Caching
{
    public class ScdRedisCacheManager : IScdCacheManager
    {
        private readonly IDatabase _db;

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

        #endregion

        public ScdRedisCacheManager()
        {
            IRedisCacheDatabaseProvider databaseProvider = new RedisCacheDatabaseProvider();
            _db = databaseProvider.GetDatabase();
        }

        public TResult Get<TResult>(ScdCacheKey key)
        {
            var rValue = _db.StringGet(key.ToString());
            if (!rValue.HasValue)
            {
                // 此处写入文件日志计算Redis的key以及操作对应的次数
                XFileLoger.WriteLogEx("RedisCacheGet_" + key, "NoData");
                return default(TResult);
            }
            // 此处写入文件日志计算Redis的key以及操作对应的次数
            XFileLoger.WriteLogEx("RedisCacheGet_" + key, CommonHelper.ConvertBytes(((byte[])rValue).Length));
            var result = Deserialize<TResult>(rValue);
            return result;
        }

        public void Set(ScdCacheKey key, object data, double cacheTime)
        {
            if (data == null)
                return;

            // 此处写入文件日志计算Redis的key以及操作对应的次数
            var entryBytes = Serialize(data);
            XFileLoger.WriteLogEx("RedisCacheSet_" + key, CommonHelper.ConvertBytes(entryBytes.Length));
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            _db.StringSet(key.ToString(), entryBytes, expiresIn);
        }

        public bool IsSet(ScdCacheKey key)
        {
            return _db.KeyExists(key.ToString());
        }

        public void Remove(ScdCacheKey key)
        {
            _db.KeyDelete(key.ToString());
        }
    }


    public class ScdRedisNullCacheManager : IScdCacheManager
    {
        private readonly IDatabase _db;

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

        #endregion

        public ScdRedisNullCacheManager()
        {
            IRedisCacheDatabaseProvider databaseProvider = new RedisCacheDatabaseProvider();
            _db = databaseProvider.GetDatabase();
        }

        public TResult Get<TResult>(ScdCacheKey key)
        {
            // 此处写入文件日志计算Redis的key以及操作对应的次数
            XFileLoger.WriteLogEx("RedisNullCacheGet_" + key, "OneData");
            var rValue = _db.StringGet(key.ToString());
            if (rValue.HasValue)
            {
                byte[] byteData = rValue;
                if (byteData.Length > 0)
                {
                    // 此处写入文件日志计算Redis的key以及操作对应的次数
                    XFileLoger.WriteLogEx("RedisNullCacheGet_" + key, CommonHelper.ConvertBytes(byteData.Length));
                    var result = Deserialize<TResult>(rValue);
                    return result;
                }
            }
            // 此处写入文件日志计算Redis的key以及操作对应的次数
            XFileLoger.WriteLogEx("RedisCacheGet_" + key, "NoData");
            return default(TResult);
        }

        public void Set(ScdCacheKey key, object data, double cacheTime)
        {
            // 此处写入文件日志计算Redis的key以及操作对应的次数
            var entryBytes = data == null ? new byte[0] : Serialize(data);
            XFileLoger.WriteLogEx("RedisNullCacheSet_" + key, CommonHelper.ConvertBytes(entryBytes.Length));
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            _db.StringSet(key.ToString(), entryBytes, expiresIn);
        }

        public bool IsSet(ScdCacheKey key)
        {
            return _db.KeyExists(key.ToString());
        }

        public void Remove(ScdCacheKey key)
        {
            _db.KeyDelete(key.ToString());
        }
    }

}
