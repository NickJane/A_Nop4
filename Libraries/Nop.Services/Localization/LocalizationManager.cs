using System;
using System.Collections.Generic;
using System.Globalization;
using Nop.Services.Localization;
using System.IO;

namespace Nop.Services.Localization
{
    internal class LocalizationManager
    {
        /// <summary>
        /// 系统所有资源文件的集合
        /// </summary>
        private Dictionary<string, ILocalizationDictionary> allResourceDictionary;
        private readonly string _localizationDirectoryPath;
        private static readonly object _sync = new object();
        private bool _isInited;
        private string resourcefilleKeyFormat = "{0}-{1}";//LocalizationDictionaryName-zh-cn

        public LocalizationManager(string localizationDirectoryPath)
        {
            if (string.IsNullOrEmpty(localizationDirectoryPath))
            {
                throw new ArgumentException("localizationDirectoryPath can not be null or empty");
            }
            _localizationDirectoryPath = localizationDirectoryPath;
            allResourceDictionary = new Dictionary<string, ILocalizationDictionary>();
            InitSources();
        }

        private void InitSources()
        {
            lock (_sync)
            {
                if (_isInited)
                {
                    return;
                }
                var fileNames = System.IO.Directory.GetFiles(_localizationDirectoryPath, "*.json", SearchOption.TopDirectoryOnly);

                foreach (var fileName in fileNames)
                {
                    var dictionary = JsonLocalizationDictionaryHelper.BuildFromFile(fileName);
                    string key = string.Format(resourcefilleKeyFormat, dictionary.DictionaryName ,dictionary.CultureInfo.Name);
                    if (allResourceDictionary.ContainsKey(key))
                    {
                        throw new NopException(dictionary.DictionaryName +
                                                  " source contains more than one dictionary for the culture: " +
                                                  dictionary.CultureInfo.Name);
                    }

                    allResourceDictionary[key] = dictionary; 
                }
                _isInited = true;
            }
        }

        public string GetString(LocalizationDictionaryName type, string key, CultureInfo culture)
        {
            return GetString(type, key, culture, new object[] { });
        }

        public string GetString(LocalizationDictionaryName type, string key, CultureInfo culture, object[] args)
        {
            string resourcefilleKey = string.Format(resourcefilleKeyFormat, type.ToString(), culture.Name);

            if (allResourceDictionary.ContainsKey(resourcefilleKey)) {
                var curr = allResourceDictionary[resourcefilleKey];
                if (args == null || args.Length == 0)
                    return curr[key];
                else
                    return string.Format(curr[key], args);
            }
            return null;
        }
    }
}
