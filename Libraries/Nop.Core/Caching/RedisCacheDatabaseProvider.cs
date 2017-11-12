using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using StackExchange.Redis;
using System;

namespace Nop.Core.Caching
{
    public class RedisCacheDatabaseProvider : IRedisCacheDatabaseProvider
    {
        private static readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;
        private readonly int _databaseId;
        /// <summary>
        /// Initializes a new instance of the <see cref="ENopRedisCacheDatabaseProvider"/> class.
        /// </summary>
        public RedisCacheDatabaseProvider()
        {
            _databaseId = -1;
        }

        public RedisCacheDatabaseProvider(int databaseId)
        {
            if (databaseId <= 0)
            {
                throw new ArgumentException("databaseId must be large than zero");
            }
            _databaseId = databaseId;
        }

        static RedisCacheDatabaseProvider()
        {
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase(GetDatabaseId());
        }

        private static ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(GetConnectionString());
        }

        private int GetDatabaseId()
        {
            return _databaseId;
        }

        private static string GetConnectionString()
        {
            var globalSettings = EngineContext.Current.Resolve<GlobalSettings>();
            return globalSettings.RedisConnectionString;
        }
    }
}
