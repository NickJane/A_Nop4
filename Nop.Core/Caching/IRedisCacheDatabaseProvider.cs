using StackExchange.Redis;

namespace Nop.Core.Caching
{
    public interface IRedisCacheDatabaseProvider
    {
        /// <summary>
        /// Gets the database connection.
        /// </summary>
        IDatabase GetDatabase();
    }
}
