using System.Globalization;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Data.Domain.Localization;

namespace Nop.Services.Localization
{
     public static class LocalizationHelper
    {
        private static readonly ILocalizationManager LocalizationManager;

        static LocalizationHelper()
        {
            string directoryPath = CommonHelper.MapPath("/Localization/SourceFiles");
            LocalizationManager = new LocalizationManager(directoryPath);
        }
        /// <summary>
        /// Gets a localized string in specified language.
        /// </summary>
        /// <param name="sourceName">name of the source</param>
        /// <param name="key">localized string key</param>
        /// <param name="language">language name</param>
        /// <returns></returns>
        public static string GetString(string sourceName, string key, string language)
        {
            return LocalizationManager.GetString(sourceName, key, language);
        }
        /// <summary>
        /// Gets a localized string in specified language.
        /// </summary>
        /// <param name="sourceName">name of the source</param>
        /// <param name="key">localized string key</param>
        /// <param name="language">language name</param>
        /// <param name="args">string fromat args</param>
        /// <returns></returns>
        public static string GetString(string sourceName, string key, string language, params object[] args)
        {

            return LocalizationManager.GetString(sourceName, key, language, args);
        }
        /// <summary>
        /// Gets a localized string in specified language.
        /// </summary>
        /// <param name="sourceName">name of the source</param>
        /// <param name="key">localized string key</param>
        /// <param name="culture">culture</param>
        /// <returns></returns>
        public static string GetString(string sourceName, string key, CultureInfo culture)
        {
            return LocalizationManager.GetString(sourceName, key, culture);
        }
        /// <summary>
        /// Gets a localized string in specified language.
        /// </summary>
        /// <param name="sourceName">name of the source</param>
        /// <param name="key">localized string key</param>
        /// <param name="culture">culture</param>
        /// <param name="args">string fromat args</param>
        /// <returns></returns>
        public static string GetString(string sourceName, string key, CultureInfo culture, params object[] args)
        {
            return LocalizationManager.GetString(sourceName, key, culture, args);
        }

        public static string T(string key)
        {
            return GetString(LocalizationResourceType.Admin.ToString(), key, EngineContext.Current.Resolve<IWorkContext>().AdminLanguage.LanguageCulture);
        }

        public static string T(string key, params object[] args)
        {
            return GetString(LocalizationResourceType.Admin.ToString(), key, EngineContext.Current.Resolve<IWorkContext>().AdminLanguage.LanguageCulture, args);
        }

        public static string TC(string key)
        {
            return GetString(LocalizationResourceType.Control.ToString(), key, EngineContext.Current.Resolve<IWorkContext>().AdminLanguage.LanguageCulture);
        }
        public static string TC(string key, params object[] args)
        {
            return GetString(LocalizationResourceType.Control.ToString(), key, EngineContext.Current.Resolve<IWorkContext>().AdminLanguage.LanguageCulture, args);
        }
        public static string TD(string key)
        {
            return GetString(LocalizationResourceType.Design.ToString(), key, EngineContext.Current.Resolve<IWorkContext>().AdminLanguage.LanguageCulture);
        }
        public static string TD(string key, params object[] args)
        {
            return GetString(LocalizationResourceType.Design.ToString(), key, EngineContext.Current.Resolve<IWorkContext>().AdminLanguage.LanguageCulture, args);
        }

        public static string L(string key)
        {
            return GetString(LocalizationResourceType.Brower.ToString(), key, EngineContext.Current.Resolve<IWorkContext>().RunTimeLanguage.LanguageCulture);
        }

        public static string L(string key, params object[] args)
        {
            return GetString(LocalizationResourceType.Brower.ToString(), key, EngineContext.Current.Resolve<IWorkContext>().RunTimeLanguage.LanguageCulture, args);
        }
    }
}
