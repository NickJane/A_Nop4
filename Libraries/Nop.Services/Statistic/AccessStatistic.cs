using Nop.Core.Caching;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Statistic
{
    public class DataPoint
    {
        public long Value { get; set; }
        public DateTime Time { get; set; }
    }
    public static class AccessStatistic
    { 

        private static IRedisCacheDatabaseProvider GetRedis()
        {
            //database Id 为10，databaseId 不要重复
            return new RedisCacheDatabaseProvider(10);
        }

        //把时间转换成byte数组
        private static byte[] ConvertDateTimeToByte(DateTime time)
        {
            byte[] tbytes = new byte[4];
            Buffer.BlockCopy(BitConverter.GetBytes((short)time.Year), 0, tbytes, 0, 2);
            byte day = (byte)time.Day;
            byte month = (byte)time.Month;
            tbytes[2] = month;
            tbytes[3] = day;
            return tbytes;
        }
        //站点和时间表示某个站点在某一天的访问记录, 得到一个bytes[]作为redis的key
        #region 样例
        private static byte[] GetIpKey(int siteId, DateTime time)
        {
            byte[] key = new byte[10];//10个字节, i,p, siteId(int为4个), DateTime(日期长度为4个)
            key[0] = (byte)'i';
            key[1] = (byte)'p';
            Buffer.BlockCopy(BitConverter.GetBytes(siteId), 0, key, 2, 4);
            Buffer.BlockCopy(ConvertDateTimeToByte(time), 0, key, 6, 4);
            return key;
        }

        private static byte[] GetPvKey(int siteId, DateTime time)
        {
            byte[] key = new byte[10];
            key[0] = (byte)'p';
            key[1] = (byte)'v';
            Buffer.BlockCopy(BitConverter.GetBytes(siteId), 0, key, 2, 4);
            Buffer.BlockCopy(ConvertDateTimeToByte(time), 0, key, 6, 4);
            return key;
        } 
        #endregion

        #region ip
        public static IEnumerable<DataPoint> GetIp(int siteId, DateTime start, DateTime end)
        {
            var _redisProvider = GetRedis();

            DateTime newStart = new DateTime(start.Year, start.Month, start.Day);
            DateTime newEnd = newStart.AddDays(15);
            if (end > newEnd)
            {
                throw new ArgumentException("time distance must between 15 days");
            }
            List<DataPoint> points = new List<DataPoint>();
            int days = end.Subtract(newStart).Days;
            for (int i = 0; i <= days; i++)
            {
                DataPoint point = new DataPoint();
                point.Time = newStart.AddDays(i);
                byte[] key = GetIpKey(siteId, point.Time);
                try
                {
                    if (point.Time.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd"))
                    {
                        point.Value = _redisProvider.GetDatabase().HyperLogLogLength(key);
                    }
                    else
                    {
                        point.Value = (long)_redisProvider.GetDatabase().StringGet(key);
                    }
                }
                catch
                {
                    point.Value = 0;
                }
                points.Add(point);
            }
            return points;
        }

        public static async Task<bool> IncrementIPAsync(int siteId, string IP, DateTime dateTime)
        {
            var _redisProvider = GetRedis();  
            byte[] key = GetIpKey(siteId, dateTime);
            return await _redisProvider.GetDatabase().HyperLogLogAddAsync(key, IP);
        }
        public static bool IncrementIP(int siteId, string IP, DateTime dateTime)
        {
            return IncrementIPAsync(siteId, IP, dateTime).Result;
        }
        #endregion

        #region pv

        public static IEnumerable<DataPoint> GetPv(int siteId, DateTime start, DateTime end)
        {
            var _redisProvider = GetRedis();

            DateTime newStart = new DateTime(start.Year, start.Month, start.Day);
            DateTime newEnd = newStart.AddDays(15);
            if (end > newEnd)
            {
                throw new ArgumentException("time distance must between 15 days");
            }
            List<DataPoint> points = new List<DataPoint>();
            int days = end.Subtract(newStart).Days;
            for (int i = 0; i <= days; i++)
            {
                DataPoint point = new DataPoint();
                point.Time = newStart.AddDays(i);
                byte[] key = GetPvKey(siteId, point.Time);
                try
                {
                    point.Value = (long)_redisProvider.GetDatabase().StringGet(key);
                }
                catch
                {
                    point.Value = 0;
                }
                points.Add(point);
            }
            return points;
        }

        public static  long IncrementPv(int siteId, DateTime dateTime)
        {
            return IncrementPvAsync(siteId, dateTime).Result;
        }

        public static Task<long> IncrementPvAsync(int siteId, DateTime dateTime)
        {
            var _redisProvider = GetRedis();
            byte[] key = GetPvKey(siteId, dateTime);
            return _redisProvider.GetDatabase().StringIncrementAsync(key);
        }
        #endregion
    }
}
