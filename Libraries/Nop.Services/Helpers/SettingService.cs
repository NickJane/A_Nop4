using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace Nop.Services.Helpers
{
    /// <summary>
    /// Setting manager
    /// </summary>
    public partial class SettingService : ISettingService
    { 
        /// <summary>
        /// 缓存全局配置文件的路径
        /// </summary>
        private const string c_XmlCacheDependency = "~/app_data/globalSetting.xml";
         

        //private readonly IRepository<Setting> _settingRepository;
        //private readonly IEventPublisher _eventPublisher;
        private static readonly MemoryCacheManager _cacheManager = new MemoryCacheManager();
         
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="settingRepository">Setting repository</param>
        public SettingService(
            //, IEventPublisher eventPublisher,
            //IRepository<Setting> settingRepository
            )
        {
            //this._eventPublisher = eventPublisher;
            //this._settingRepository = settingRepository;
        }

        #endregion

        #region Nested classes

        [Serializable]
        public class SettingForCaching
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public int StoreId { get; set; }

            public void LoadByXmlNode(XmlNode node)
            {
                Id = 0;
                Name = Utilities.SafeGetAttribute(node, "key").ToLower();
                Value = Utilities.SafeGetAttribute(node, "value");
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 从XML文件中读取字符串，转换成字典，并缓存起来
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, IList<SettingForCaching>> GetAllGlobalSettingsCached()
        {
            var xmlCacheDependency = CommonHelper.MapPath(c_XmlCacheDependency);
            var cacheKey = new ScdCacheKey(ScdCacheCategory.ConfigFile, xmlCacheDependency.Replace("/", "_"));
            Dictionary<string, IList<SettingForCaching>> dictionary
                = _cacheManager.Get<Dictionary<string, IList<SettingForCaching>>>(cacheKey);
            if (dictionary == null)
            {
                string fileName = Utilities.MapPath(c_XmlCacheDependency);
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                XmlNodeList nodes = doc.DocumentElement.SelectNodes("i");
                dictionary = new Dictionary<string, IList<SettingForCaching>>();
                foreach (XmlNode node in nodes)
                {
                    SettingForCaching settingForCaching = new SettingForCaching();
                    settingForCaching.LoadByXmlNode(node);
                    var resourceName = settingForCaching.Name;
                    if (!dictionary.ContainsKey(resourceName))
                    {
                        //first setting
                        dictionary.Add(resourceName, new List<SettingForCaching>()
                        {
                            settingForCaching
                        });
                    }
                    else
                    {
                        //already added
                        //most probably it's the setting with the same name but for some certain store (storeId > 0)
                        dictionary[resourceName].Add(settingForCaching);
                    }
                }

                //压入缓存
                _cacheManager.AddCache(cacheKey, dictionary, xmlCacheDependency);
            }
            return dictionary;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 从指定的Setting存储对象中返回单一的某个Setting
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">属性的健</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="storeId"></param>
        /// <param name="loadSharedValueIfNotFound"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static T GetSettingByKey<T>(string key, T defaultValue, bool loadSharedValueIfNotFound, IDictionary<string, IList<SettingForCaching>> settings)
        {
            if (!string.IsNullOrEmpty(key))
                key = key.ToLower();
            if (settings.ContainsKey(key))
            {
                var settingsByKey = settings[key];
                //var setting = settingsByKey.FirstOrDefault(x => x.StoreId == storeId);

                var setting = settingsByKey.FirstOrDefault();

                //load shared value?
                if (setting == null && loadSharedValueIfNotFound)
                    setting = settingsByKey.FirstOrDefault();

                if (setting != null)
                    return CommonHelper.To<T>(setting.Value);
            }

            return defaultValue;
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="storeId">Store identifier for which settigns should be loaded</param>
        public virtual T LoadSetting<T>() where T : ISettings, new()
        {

            var settings = Activator.CreateInstance<T>();

            IDictionary<string, IList<SettingForCaching>> dataSource =
                GetAllGlobalSettingsCached();

            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = typeof(T).Name + "." + prop.Name;
                //load by store
                string setting = GetSettingByKey<string>(key, string.Empty, true, dataSource);
                if (setting == null)
                    continue;

                if (!CommonHelper.GetNopCustomTypeConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!CommonHelper.GetNopCustomTypeConverter(prop.PropertyType).IsValid(setting))
                    continue;

                object value = CommonHelper.GetNopCustomTypeConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                //set property
                prop.SetValue(settings, value, null);
            }

            return settings;
        }

        #endregion


    }
}