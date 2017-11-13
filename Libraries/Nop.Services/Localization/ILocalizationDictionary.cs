using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Localization
{
    /// <summary>
    /// 每种语言的资源文件集合
    /// </summary>
    public interface ILocalizationDictionary
    {
        /// <summary>
        /// Culture of the dictionary.
        /// </summary>
        CultureInfo CultureInfo { get; }
        /// <summary>
        /// Name of dictionary
        /// </summary>
        string DictionaryName { get; }

        /// <summary>
        /// Gets/sets a string for this dictionary with given name (key).
        /// </summary>
        /// <param name="name">Name to get/set</param>
        string this[string name] { get; set; }

        /// <summary>
        /// Gets a <see cref="LocalizedString"/> for given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name (key) to get localized string</param>
        /// <returns>The localized string or null if not found in this dictionary</returns>
        LocalizedString GetOrNull(string name);

        /// <summary>
        /// Gets a list of all strings in this dictionary.
        /// </summary>
        /// <returns>List of all <see cref="LocalizedString"/> object</returns>
        IReadOnlyList<LocalizedString> GetAllStrings();
    }
}
