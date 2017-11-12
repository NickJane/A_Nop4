using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Localization
{
    public interface ILocalizationManager
    {
        /// <summary>
        /// Gets a localization source with name.
        /// </summary>
        /// <param name="sourceName">name of the source</param>
        /// <returns></returns>
        ILocalizationSource GetSource(string sourceName);
        /// <summary>
        /// Gets a localized string in specified language.
        /// </summary>
        /// <param name="sourceName">name of the source</param>
        /// <param name="key">localized string key</param>
        /// <param name="language">language name</param>
        /// <returns></returns>
        string GetString(string sourceName, string key, string language);
        /// <summary>
        /// Gets a localized string in specified language.
        /// </summary>
        /// <param name="sourceName">name of the source</param>
        /// <param name="key">localized string key</param>
        /// <param name="language">language name</param>
        /// <param name="args">string fromat args</param>
        /// <returns></returns>
        string GetString(string sourceName, string key, string language, object[] args);
        /// <summary>
        /// Gets a localized string in specified language.
        /// </summary>
        /// <param name="sourceName">name of the source</param>
        /// <param name="key">localized string key</param>
        /// <param name="culture">culture</param>
        /// <returns></returns>
        string GetString(string sourceName, string key, CultureInfo culture);
        /// <summary>
        /// Gets a localized string in specified language.
        /// </summary>
        /// <param name="sourceName">name of the source</param>
        /// <param name="key">localized string key</param>
        /// <param name="culture">culture</param>
        /// <param name="args">string fromat args</param>
        /// <returns></returns>
        string GetString(string sourceName, string key, CultureInfo culture, object[] args);
    }
}
