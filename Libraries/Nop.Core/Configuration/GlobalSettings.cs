namespace Nop.Core.Configuration
{
    public class GlobalSettings : ISettings
    {
        /// <summary>
        /// redis的连接字符串
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// OSS路径开关，值为on/off，为on的时候，代表js、css等文件从OSS中获取
        /// </summary>
        private string OSSPathSwitch { get; set; }

       
    }

}
