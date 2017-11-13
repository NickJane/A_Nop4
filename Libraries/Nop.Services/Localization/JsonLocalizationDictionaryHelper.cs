using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nop.Services.Localization
{
    public class JsonLocalizationDictionaryHelper  
    { 
        /// <summary>
        ///     Builds an <see cref="JsonLocalizationDictionary" /> from given file.
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        public static LocalizationDictionary BuildFromFile(string filePath)
        {
            try
            {
                return BuildFromJsonString(File.ReadAllText(filePath));
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid localization file format! " + filePath, ex);
            }
        }

        /// <summary>
        ///     Builds an <see cref="JsonLocalizationDictionary" /> from given json string.
        /// </summary>
        /// <param name="jsonString">Json string</param>
        public static LocalizationDictionary BuildFromJsonString(string jsonString)
        {
            JsonLocalizationFile jsonFile;
            try
            {
                jsonFile = JsonConvert.DeserializeObject<JsonLocalizationFile>(
                    jsonString,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
            }
            catch (JsonException ex)
            {
                throw new WezhanException("Can not parse json string. " + ex.Message);
            }

            var cultureCode = jsonFile.Culture;
            if (string.IsNullOrEmpty(cultureCode))
            {
                throw new WezhanException("Culture is empty in language json file.");
            }
            if (string.IsNullOrEmpty(jsonFile.DictionaryName))
            {
                throw new WezhanException("DictionaryName is empty in language json file.");
            }
            var dictionary = new LocalizationDictionary(new CultureInfo(cultureCode), jsonFile.DictionaryName);
            var dublicateNames = new List<string>();
            foreach (var item in jsonFile.Texts)
            {
                if (string.IsNullOrEmpty(item.Key))
                {
                    throw new WezhanException("The key is empty in given json string.");
                }
                 
                if (dictionary.ContainsKey(item.Key))
                {
                    dublicateNames.Add(item.Key);
                }

                dictionary[item.Key] = item.Value.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine); ;
            }

            if (dublicateNames.Count > 0)
            {
                throw new WezhanException(
                    "A dictionary can not contain same key twice. There are some duplicated names: " +
                    String.Join(", ", dublicateNames));
            }

            return dictionary;
        }
    }


    public class JsonLocalizationFile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonLocalizationFile()
        {
            Texts = new Dictionary<string, string>();
        }

        /// <summary>
        /// get or set the culture name; eg : en , en-us, zh-CN
        /// </summary>
        public string Culture { get; set; }
        /// <summary>
        /// get or set the name of current file;eg:design,control,admin
        /// </summary>
        public string DictionaryName { get; set; }

        /// <summary>
        ///  Key value pairs
        /// </summary>
        public Dictionary<string, string> Texts { get; private set; }
    }
}
