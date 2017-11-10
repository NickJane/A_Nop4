
	接口IScdCacheManager
		实现ScdPerRequestCacheManager, 使用了httpContent.items放置缓存
			ScdRedisCacheManager		redis
			ScdMemoryCacheManager		.net自带缓存
		Redis实例化的时候
		public ScdRedisNullCacheManager()
        {
            IRedisCacheDatabaseProvider databaseProvider = new RedisCacheDatabaseProvider();
            _db = databaseProvider.GetDatabase();
        }
		在构造函数内获得Redis数据库操作对象.