using System.Globalization;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Data; 

namespace Nop.Services.Localization
{
     public static class LocalizationHelper
    {
        private static readonly LocalizationManager LocalizationManager;

        static LocalizationHelper()
        {
            string directoryPath = CommonHelper.MapPath("/App_Data/Localization");
            LocalizationManager = new LocalizationManager(directoryPath);
        }
        public static string GetString(LocalizationDictionaryName dictionaryName, string key, CultureInfo culture,params object[] args)
        {
            return LocalizationManager.GetString(dictionaryName, key, culture, args);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionaryName"></param>
        /// <param name="key"></param>
        /// <param name="cultureCode">en-us, zh-cn, etc...</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetString(LocalizationDictionaryName dictionaryName, string key, string cultureCode, params object[] args)
        {
            return LocalizationManager.GetString(dictionaryName, key, new CultureInfo(cultureCode), args);
        }

        public static string T(string key)
        {
            return GetString(LocalizationDictionaryName.Admin, key, 
                EngineContext.Current.Resolve<IWorkContext>().RunTimeLanguage.LanguageCulture);
        }

        public static string T(string key, string languageCulture)
        {
            return GetString(LocalizationDictionaryName.Admin, key, languageCulture);
        }

        public static string T(string key, params object[] args)
        {
            return GetString(LocalizationDictionaryName.Admin, key, 
                EngineContext.Current.Resolve<IWorkContext>().RunTimeLanguage.LanguageCulture, args);
        }
        

        public static string L(string key)
        {
            return GetString(LocalizationDictionaryName.Web, key, 
                EngineContext.Current.Resolve<IWorkContext>().RunTimeLanguage.LanguageCulture);
        }
        public static string L(string key,string languageCulture)
        {
            return GetString(LocalizationDictionaryName.Web, key, languageCulture);
        }

        public static string L(string key, params object[] args)
        {
            return GetString(LocalizationDictionaryName.Web, key, 
                EngineContext.Current.Resolve<IWorkContext>().RunTimeLanguage.LanguageCulture, args);
        }
    }
}
