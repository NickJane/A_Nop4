namespace Nop.Core.Caching
{
    public interface ICacheManager
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TResult Get<TResult>(ScdCacheKey key);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime">缓存时间，单位是分钟</param>
        void Set(ScdCacheKey key, object data, double cacheTime);

        /// <summary>
        /// 是否已经设置过缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsSet(ScdCacheKey key);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(ScdCacheKey key);
    }

    
}
