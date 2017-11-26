using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Localization
{
    public class LocalizationDictionary : ILocalizationDictionary, IEnumerable<LocalizedString>
    {

        /// <inheritdoc/>
        public CultureInfo CultureInfo { get; private set; }
        public string DictionaryName { get; private set; }

        private readonly Dictionary<string, LocalizedString> _dictionary;

        /// <inheritdoc/>
        public virtual string this[string name]
        {
            get
            {
                var localizedString = GetOrNull(name);
                return localizedString == null ? null : localizedString.Value;
            }
            set
            {
                _dictionary[name] = new LocalizedString(name, value, CultureInfo);
            }
        }


        /// <summary>
        /// Creates a new <see cref="LocalizationDictionary"/> object.
        /// </summary>
        /// <param name="cultureInfo">Culture of the dictionary</param>
        /// <param name="dictionaryName">Name of the dictionary</param>
        public LocalizationDictionary(CultureInfo cultureInfo, string dictionaryName)
        {
            CultureInfo = cultureInfo;
            _dictionary = new Dictionary<string, LocalizedString>();
            DictionaryName = dictionaryName;
        }

        /// <inheritdoc/>
        public virtual LocalizedString GetOrNull(string name)
        {
            LocalizedString localizedString;
            return _dictionary.TryGetValue(name, out localizedString) ? localizedString : null;
        }

        /// <inheritdoc/>
        public virtual IReadOnlyList<LocalizedString> GetAllStrings()
        {
            return _dictionary.Values.ToList();
        }

        /// <inheritdoc/>
        public virtual IEnumerator<LocalizedString> GetEnumerator()
        {
            return GetAllStrings().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAllStrings().GetEnumerator();
        }

        public bool ContainsKey(string name)
        {
            return _dictionary.ContainsKey(name);
        }
    }
}
