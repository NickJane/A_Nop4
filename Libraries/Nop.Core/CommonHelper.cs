using Nop.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace Nop.Core
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// Ensures the subscriber email or throw.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            string output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!output.IsEmail())
            {
                throw new NopException("Email is not valid.");
            }

            return output;
        }
        

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var pLen = postfix == null ? 0 : postfix.Length;

                var result = str.Substring(0, maxLength - pLen);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }

            return str;
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : new string(str.Where(p => char.IsDigit(p)).ToArray());
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            return str ?? string.Empty;
        }

        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            return stringsToValidate.Any(p => string.IsNullOrEmpty(p));
        }

        /// <summary>
        /// Compare two arrasy
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="a1">Array 1</param>
        /// <param name="a2">Array 2</param>
        /// <returns>Result</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            //also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }

        private static AspNetHostingPermissionLevel? _trustLevel;
        /// <summary>
        /// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        /// </summary>
        /// <returns>The current trust level.</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (!_trustLevel.HasValue)
            {
                //set minimum
                _trustLevel = AspNetHostingPermissionLevel.None;

                //determine maximum
                foreach (AspNetHostingPermissionLevel trustLevel in new[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal
                            })
                {
                    try
                    {
                        new AspNetHostingPermission(trustLevel).Demand();
                        _trustLevel = trustLevel;
                        break; //we've set the highest permission we can
                    }
                    catch (System.Security.SecurityException)
                    {
                        continue;
                    }
                }
            }
            return _trustLevel.Value;
        }

        /// <summary>
        /// Sets a property on an object to a valuae.
        /// </summary>
        /// <param name="instance">The object whose property to set.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            Type instanceType = instance.GetType();
            PropertyInfo pi = instanceType.GetProperty(propertyName);
            if (pi == null)
                throw new NopException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);
            if (!pi.CanWrite)
                throw new NopException("The property '{0}' on the instance of type '{1}' does not have a setter.", propertyName, instanceType);
            if (value != null && !value.GetType().IsAssignableFrom(pi.PropertyType))
                value = To(value, pi.PropertyType);
            pi.SetValue(instance, value, new object[0]);
        }

        public static TypeConverter GetScdCustomTypeConverter(Type type)
        {
            ////we can't use the following code in order to register our custom type descriptors
            ////TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            ////so we do it manually here

            //if (type == typeof(List<int>))
            //    return new GenericListTypeConverter<int>();
            //if (type == typeof(List<decimal>))
            //    return new GenericListTypeConverter<decimal>();
            //if (type == typeof(List<string>))
            //    return new GenericListTypeConverter<string>();
            //if (type == typeof(ShippingOption))
            //    return new ShippingOptionTypeConverter();
            //if (type == typeof(List<ShippingOption>) || type == typeof(IList<ShippingOption>))
            //    return new ShippingOptionListTypeConverter();
            //if (type == typeof(PickupPoint))
            //    return new PickupPointTypeConverter();
            //if (type == typeof(Dictionary<int, int>))
            //    return new GenericDictionaryTypeConverter<int, int>();

            //return TypeDescriptor.GetConverter(type);

            return null;
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                TypeConverter destinationConverter = GetScdCustomTypeConverter(destinationType);
                TypeConverter sourceConverter = GetScdCustomTypeConverter(sourceType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);
                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);
                if (!destinationType.IsInstanceOfType(value))
                    return Convert.ChangeType(value, destinationType, culture);
            }
            return value;
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }

        public static TypeConverter GetNopCustomTypeConverter(Type type)
        {
            //we can't use the following code in order to register our custom type descriptors
            //TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            //so we do it manually here

            if (type == typeof(List<int>))
                return new GenericListTypeConverter<int>();
            if (type == typeof(List<decimal>))
                return new GenericListTypeConverter<decimal>();
            if (type == typeof(List<string>))
                return new GenericListTypeConverter<string>();

            return TypeDescriptor.GetConverter(type);
        }

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            string result = string.Empty;
            foreach (var c in str)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();
            return result;
        }

        /// <summary>
        /// Set Telerik (Kendo UI) culture
        /// </summary>
        public static void SetTelerikCulture()
        {
            //little hack here
            //always set culture to 'en-US' (Kendo UI has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs

            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Get difference in years
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            //source: http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
            //this assumes you are looking for the western idea of age and not using East Asian reckoning.
            int age = endDate.Year - startDate.Year;
            if (startDate > endDate.AddYears(-age))
                age--;
            return age;
        }

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }

            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }
       

        public static int TrimToInt(string str, string trimStr, int defaultValue = 0)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return defaultValue;
            }
            int value;
            if (!int.TryParse(str.Replace(trimStr, string.Empty), out value))
            {
                value = defaultValue;
            }
            return value;
        }
       

        /// <summary>
        /// 数据大小转换字节数
        /// g,gb,m,mb,k,kb->bytes
        /// </summary>
        /// <param name="sizecount"></param>
        /// <returns></returns>
        public static double DataConvertToByte(string sizecount)
        {
            string sizeString = sizecount.ToLower();
            Double dataSize = 1;
            if (sizeString.EndsWith("g") || sizeString.EndsWith("gb"))
            {
                sizeString = sizeString.Replace("gb", "").Replace("g", "");
                dataSize = Convert.ToDouble(sizeString);
                dataSize = dataSize * 1024 * 1024 * 1024;
            }
            else if (sizeString.EndsWith("m") || sizeString.EndsWith("mb"))
            {
                sizeString = sizeString.Replace("mb", "").Replace("m", "");
                dataSize = Convert.ToDouble(sizeString);
                dataSize = dataSize * 1024 * 1024;
            }
            else if (sizeString.EndsWith("k") || sizeString.EndsWith("kb"))
            {
                sizeString = sizeString.Replace("kb", "").Replace("k", "");
                dataSize = Convert.ToDouble(sizeString);
                dataSize = dataSize * 1024;
            }
            return dataSize;
        }

        public static string GetSignature(string timestamp, string nonce, string token = null, IEnumerable<string> customArray = null)
        {
            List<string> result = new List<string>();
            result.Add(timestamp);
            result.Add(nonce);
            result.Add(token);
            if (customArray != null)
            {
                result.AddRange(customArray);
            }
            var arr = result.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
        }
       

        public static string GetMimeType(string fileExt)
        {
            fileExt = fileExt.ToLower();
            string ext = fileExt.Replace(".", "");
            string result = "application/" + ext;
            switch (ext)
            {
                case "ppt":
                case "pptx":
                    result = "application/vnd.ms-powerpoint";
                    break;
                case "doc":
                case "docx":
                    result = "application/msword";
                    break;
                case "xls":
                case "xlsx":
                case "xla":
                case "xlc":
                case "xlm":
                    result = "application/vnd.ms-excel";
                    break;
                case "z":
                case "tgz":
                    result = "application/x-compress";
                    break;
                case "htm":
                case "html":
                    result = "text/html";
                    break;
                case "txt":
                case "c":
                case "h":
                    result = "text/plain";
                    break;
                case "jpg":
                case "jpeg":
                case "png":
                case "bmp":
                    result = "image/" + ext;
                    break;
                case "gif":
                    result = "image/gif";
                    break;
                case "swf":
                    result = "application/x-shockwave-flash";
                    break;
                case "m4a":
                    result = "audio/mp4a-latm";
                    break;
                case "oga":
                case "ogg":
                    result = "audio/ogg";
                    break;
                case "flac":
                    result = "audio/flac";
                    break;
                case "wav":
                    result = "audio/x-wav";
                    break;
                case "fla":
                    result = "audio/fla";
                    break;
                case "rar":
                    result = "application/x-rar-compressed";
                    break;
                case "apk":
                    result = "application/vnd.android.package-archive";
                    break;
                case ".ipa":
                case ".air":
                    result = "application/octet-stream";
                    break;
                case "zip":
                    result = "application/zip";
                    break;
                case "mp3":
                case "mp4":
                case "mpe":
                case "mpeg":
                    result = "audio/" + ext;
                    break;
                case "m4v":
                    result = "video/x-m4v";
                    break;
                case "ogv":
                    result = "video/ogv";
                    break;
                case "webmv":
                    result = "video/webm";
                    break;
                case "flv":
                    result = "application/octet-stream";
                    break;
                case "rtmpv":
                    result = "video/rtmp";
                    break;
                case "js":
                    result = "text/javascript";
                    break;
                case "css":
                    result = "text/css";
                    break;
            }
            return result;
        }
       
        
        public static string GetNewFileType(string fileExt)
        {
            if (string.IsNullOrWhiteSpace(fileExt))
            {
                return "";
            }
            fileExt = fileExt.Trim().ToLower();
            string ext = fileExt.Replace(".", "");
            string newType = string.Empty;
            switch (ext)
            {
                case "mp3":
                case "cd":
                case "wav":
                case "aiff":
                case "au":
                case "wma":
                case "ogg":
                case "mp3pro":
                case "real":
                case "ape":
                case "module":
                case "midi":
                case "vqf":
                case "flac":
                    newType = "Admin.FileUpload.List.Types.Music";
                    break;
                case "ini":
                case "config":
                case "conf":
                case "xls":
                case "xlsx":
                case "cs":
                case "apk":
                case "html":
                case "htm":
                case "eml":
                case "pdf":
                case "ppt":
                case "pptx":
                case "txt":
                case "doc":
                case "docx":
                    newType = "Admin.FileUpload.List.Types.Txt";
                    break;
                case "png":
                case "jpg":
                case "jpeg":
                case "ico":
                case "bmp":
                case "gif":
                case "psd":
                case "raw":
                case "tiff":
                case "pcx":
                case "tga":
                case "exif":
                case "fpx":
                case "svg":
                case "cdr":
                case "pcd":
                case "dxf":
                case "ufo":
                case "eps":
                case "ai":
                case "hdri":
                    newType = "Admin.FileUpload.List.Types.Img";
                    break;
                case "avi":
                case "rmvb":
                case "rm":
                case "asf":
                case "divx":
                case "mpg":
                case "mpeg":
                case "mpe":
                case "wmv":
                case "mp4":
                case "mkv":
                case "vob":
                case "swf":
                case "flv":
                    newType = "Admin.FileUpload.List.Types.Video";
                    break;

                case "zip":
                case "rar":
                case "7z":
                    newType = "Admin.FileUpload.List.Types.Rar";
                    break;
                default:
                    newType = "Admin.FileUpload.List.Types.Other";
                    break;
            }
            return newType;
        }
       

        private const string sourceText = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string GetRandomPassword(int length = 6)
        {
            var result = new StringBuilder();
            var random = new Random(Environment.TickCount);
            for (var i = 0; i < length; i++)
            {
                var index = random.Next(0, sourceText.Length - 1);
                result.Append(sourceText[index]);
            }
            return result.ToString();
        }

        /// <summary>
        /// 生成16位字符串
        /// </summary>
        /// <returns></returns>
        public static string GetMd5String(params string[] keys)
        {
            if (keys == null)
            {
                return string.Empty;
            }
            StringBuilder keyBuilder = new StringBuilder();
            foreach (var key in keys)
            {
                keyBuilder.Append(key);
            }

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(keyBuilder.ToString()));
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < t.Length; i++)
            {
                strBuilder.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return strBuilder.ToString(0, 16);
        }
        public static string GetRandomDomain(string suffixes, string defaultSiteDomain, string vmArea)
        {
            string guid = GetRandomGuid();
            var hash = "." + EncryptionHash(guid.GetHashCode() % 100);
            if (!string.IsNullOrWhiteSpace(suffixes))
            {
                return guid + hash + suffixes;
            }
            if (!string.IsNullOrWhiteSpace(vmArea))
            {
                vmArea = ("." + vmArea).ToLower();
            }
            return string.Format("{0}{1}{2}{3}", guid, hash, vmArea, defaultSiteDomain);
        }
        public static string GetRandomGuid()
        {
            string guid = Guid.NewGuid().ToString();
            return guid.GetHashCode().ToString().Replace('-', 'c');
        }

        /// <summary>
        /// 生成随机的验证码
        /// </summary>
        /// <param name="captchaLength"></param>
        /// <returns></returns>
        public static string GetRandomCaptcha(int captchaLength = 4)
        {
            string str = string.Empty;
            if (captchaLength > 0)
            {
                var random = new Random();
                string[] strArr = { "a", "b", "c", "n", "1", "2", "m", "3", "4", "5", "6", "7", "8", "9", "0", "o", "p", "q" };
                for (int i = 0; i < captchaLength; i++)
                {
                    int index = random.Next(strArr.Length);
                    str += strArr[index];
                }
            }
            return str;
        }

        /// <summary>
        /// 读取txt文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line);
                }
                sr.Dispose();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 读取邮件模版
        /// </summary>
        /// <param name="templeteFileName">模版名称(不带.html)后缀</param>
        /// <param name="languageCulture">语言</param>
        /// <returns></returns>
        public static string ReadEmailTemplete(string templeteFileName, string languageCulture)
        {
            //languageCulture
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\EMailTemplete\" + templeteFileName + "_" + languageCulture + ".html");
            if (!File.Exists(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\EMailTemplete\" + templeteFileName + ".html");
            }

            if (!File.Exists(path))
            {
                return "";
            }
            return ReadFile(path);
        }

        /// <summary>
        /// 验证码的有效期5分钟
        /// </summary>
        /// <returns></returns>
        public static int CaptchaTimeout()
        {
            return 5;
        }

        public static string ConvertBytes(double len)
        {
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public static string GetFileTypeImg(string fileType)
        {
            if (string.IsNullOrWhiteSpace(fileType))
            {
                fileType = "";
            }
            fileType = fileType.Trim().ToLower();
            string imgName;
            switch (fileType)
            {
                case ".mp3":
                case ".cd":
                case ".wav":
                case ".aiff":
                case ".au":
                case ".wma":
                case ".ogg":
                case ".mp3pro":
                case ".real":
                case ".ape":
                case ".module":
                case ".midi":
                case ".vqf":
                case ".flac":
                    imgName = "audio.png";
                    break;
                case ".ini":
                case ".config":
                case ".conf":
                    imgName = "conf.png";
                    break;
                case ".xls":
                case ".xlsx":
                    imgName = "excel.png";
                    break;
                case ".cs":
                case ".apk":
                    imgName = "file.png";
                    break;
                case ".html":
                case ".htm":
                    imgName = "html.png";
                    break;
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".ico":
                case ".bmp":
                case ".gif":
                case ".psd":
                case ".raw":
                case ".tiff":
                case ".pcx":
                case ".tga":
                case ".exif":
                case ".fpx":
                case ".svg":
                case ".cdr":
                case ".pcd":
                case ".dxf":
                case ".ufo":
                case ".eps":
                case ".ai":
                case ".hdri":
                    imgName = "img.png";
                    break;
                case ".eml":
                    imgName = "mail.png";
                    break;
                case ".pdf":
                    imgName = "pdf.png";
                    break;
                case ".ppt":
                case ".pptx":
                    imgName = "ppt.png";
                    break;
                case ".txt":
                    imgName = "txt.png";
                    break;
                case ".avi":
                case ".rmvb":
                case ".rm":
                case ".asf":
                case ".divx":
                case ".mpg":
                case ".mpeg":
                case ".mpe":
                case ".wmv":
                case ".mp4":
                case ".mkv":
                case ".vob":
                case ".swf":
                case ".flv":
                    imgName = "video.png";
                    break;
                case ".doc":
                case ".docx":
                    imgName = "word.png";
                    break;
                case ".zip":
                case ".rar":
                case ".7z":
                    imgName = "zip.png";
                    break;
                default:
                    imgName = "unknown.png";
                    break;
            }
            return imgName;
        }

        public static int[] StrToIntArray(string str, int[] Def)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return Def;
                }
                var idList = new List<int>();
                foreach (var intStr in str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int i;
                    if (int.TryParse(intStr, out i))
                    {
                        idList.Add(i);
                    }
                }
                return idList.ToArray();
            }
            catch (Exception)
            {
                return Def;
            }
        }

        public static string IntArrayToStr(int[] intArray)
        {
            return string.Join(",", intArray);
        }
        public static long[] StrToLongArray(string str, long[] Def)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return Def;
                }
                var idList = new List<long>();
                foreach (var intStr in str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    long i;
                    if (long.TryParse(intStr, out i))
                    {
                        idList.Add(i);
                    }
                }
                return idList.ToArray();
            }
            catch (Exception)
            {
                return Def;
            }
        }

        public static string LongArrayToStr(long[] intArray)
        {
            return string.Join(",", intArray);
        }

        #region OrderByFieldDefine

        /// <summary>
        /// 创建时间
        /// </summary>
        public const string List_OrderByField_CreateTime = "createtime";
        /// <summary>
        /// 更新时间
        /// </summary>
        public const string List_OrderByField_UpdateTime = "updatetime";
        /// <summary>
        /// 点击次数
        /// </summary>
        public const string List_OrderByField_Hits = "hits";

        public static string GetListOrderByField(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            switch (value.ToLower())
            {
                case List_OrderByField_CreateTime:
                    return List_OrderByField_CreateTime;
                case List_OrderByField_UpdateTime:
                    return List_OrderByField_UpdateTime;
                case List_OrderByField_Hits:
                    return List_OrderByField_Hits;
            }
            return string.Empty;
        }
        /// <summary>
        /// 升序排列
        /// </summary>
        public const string List_OrderByType_Asc = "asc";
        /// <summary>
        /// 降序排列
        /// </summary>
        public const string List_OrderByType_Desc = "desc";

        public static string GetListOrderByType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return List_OrderByType_Desc;
            }
            switch (value.ToLower())
            {
                case List_OrderByType_Asc:
                    return List_OrderByType_Asc;
                case List_OrderByType_Desc:
                    return List_OrderByType_Desc;
            }
            return List_OrderByType_Desc;
        }
        #endregion

        public static string GetPageTypeName(string pageTypeName)
        {
            if (string.IsNullOrEmpty(pageTypeName))
            {
                return "";
            }
            switch (pageTypeName.ToLower())
            {
                case "contentpage":
                    return "内容页面";
                case "templatepage":
                    return "页头页尾";
                case "productcontentpage":
                    return "产品详情页面";
                case "newscontentpage":
                    return "文章详情页面";
                case "productcategorypage":
                    return "产品分类结果页";
                case "newscategorypage":
                    return "文章分类结果页";
                case "productsearchpage":
                    return "产品搜索结果页";
                case "newssearchpage":
                    return "文章搜索结果页";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 得到局域网Ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIpAddress()
        {
            string hostname = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostEntry(hostname);
            string localIp = "";
            foreach (IPAddress ipAddress in localhost.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ipAddress.ToString();
                }
            }
            if (string.IsNullOrEmpty(localIp))
            {
                IPAddress localaddr = localhost.AddressList[0];
                localIp = localaddr.ToString();
            }
            return localIp;
        }

        /// <summary>
        /// 检测域名是否可以访问
        /// </summary>
        /// <param name="host">如果可以访问返回true，否则false</param>
        /// <returns></returns>
        public static bool PingHost(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return false;
            }

            try
            {
                IdnMapping map = new IdnMapping();
                host = map.GetAscii(host);
                Ping pin = new Ping();
                PingReply reply = pin.Send(host, 1200);
                if (reply != null && reply.Status == IPStatus.Success)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool PingIp(string ipAddress)
        {
            Ping pin = new Ping();
            try
            {
                IPAddress address = IPAddress.Parse(ipAddress);
                PingReply reply = pin.Send(address, 1200);
                if (reply != null && reply.Status == IPStatus.Success)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string GetTemplateLinkUrl(string linkUrl, int templateId)
        {
            if (templateId > 0)
            {
                return string.Format("{0}?templateId={1}", linkUrl, templateId);
            }
            return linkUrl;
        }
        #region PageTypeName

        public const string PageTypeName_ContentPage = "ContentPage";
        public const string PageTypeName_TemplatePage = "TemplatePage";
        public const string PageTypeName_NewsContentPage = "NewsContentPage";
        public const string PageTypeName_ProductContentPage = "ProductContentPage";
        public const string PageTypeName_NewsSearchPage = "NewsSearchPage";
        public const string PageTypeName_ProductSearchPage = "ProductSearchPage";
        public const string PageTypeName_NewsCategoryPage = "NewsCategoryPage";
        public const string PageTypeName_ProductCategoryPage = "ProductCategoryPage";

        #endregion
        public static int GetPageTypeNameId(string pageTypeName)
        {
            switch (pageTypeName)
            {
                case "NewsContentPage":
                    return 2;
                case "ProductContentPage":
                    return 3;
                case "ContentPage":
                case "TemplatePage":
                    return 1;
            }
            return 0;
        }
        public static bool IsContentPage(string pageTypeName)
        {
            if (pageTypeName == "ContentPage" || pageTypeName == "NewsContentPage" || pageTypeName == "ProductContentPage" || pageTypeName == "NewsSearchPage" || pageTypeName == "ProductSearchPage" || pageTypeName == "NewsCategoryPage" || pageTypeName == "ProductCategoryPage")
            {
                return true;
            }
            return false;
        }
        public static bool IsTemplatePage(string pageTypeName)
        {
            if (pageTypeName == "TemplatePage")
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否扩展工具栏
        /// </summary>
        /// <param name="pageTypeName"></param>
        /// <returns></returns>
        public static bool IsExtToolBar(string pageTypeName)
        {
            switch (pageTypeName)
            {
                case "NewsContentPage":
                case "ProductContentPage":
                case "NewsSearchPage":
                case "ProductSearchPage":
                case "NewsCategoryPage":
                case "ProductCategoryPage":
                    return true;
            }
            return false;
        }
        public static bool AreByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            bool areEqual = true;
            for (int i = 0; i < a.Length; i++)
            {
                areEqual &= (a[i] == b[i]);
            }
            return areEqual;
        }

        public static string GetUrlFileName(string url)
        {
            var result = "";
            if (!string.IsNullOrEmpty(url))
            {
                var dotIndex = url.LastIndexOf("/");
                if (dotIndex > -1)
                {
                    result = url.Substring(dotIndex);
                }
            }

            return result;
        }

        public static string GetFileExtName(string fileName)
        {
            var result = "";
            if (!string.IsNullOrEmpty(fileName))
            {
                var dotIndex = fileName.LastIndexOf(".");
                if (dotIndex > -1)
                {
                    result = fileName.Substring(dotIndex);
                }
            }

            return result;
        }

        public static bool IsMatchBaiduStatistics(string baiduStatisticsContent)
        {
            var content = HttpUtility.UrlDecode(baiduStatisticsContent).ToLower();
            var regexEnter = new Regex(@"[\n]+");
            var regexEnterLinux = new Regex(@"[\r\n]+");
            var regexEmpty = new Regex(@"[\s]+");
            var regexQuotes = new Regex("[\"]+");
            var regexParameter = new Regex(@"\?[A-Za-z0-9]+'");
            content = regexEnter.Replace(content, "");
            content = regexEnterLinux.Replace(content, "");
            content = regexEmpty.Replace(content, "");
            content = regexQuotes.Replace(content, "'");
            content = regexParameter.Replace(content, "?xxx'");
            var baiduStatisticsTemplete = "<script>var_hmt=_hmt||[];(function(){varhm=document.createelement('script');hm.src='https://hm.baidu.com/hm.js?xxx';vars=document.getelementsbytagname('script')[0];s.parentnode.insertbefore(hm,s);})();</script>";
            return baiduStatisticsTemplete == content;
        }
        public static bool IsMatchBaiduBridge(string baiduBridge)
        {
            return IsMatchBaiduStatistics(baiduBridge);
        }

        public static bool IsMatchSiteValidation(string siteValidationContent)
        {
            var content = HttpUtility.UrlDecode(siteValidationContent).ToLower();
            var regexQuotes = new Regex("[\"]+");
            content = regexQuotes.Replace(content, "'");
            var regexEnter = new Regex(@"<(S*?)[^>]*>.*?|<.*? /> ");
            return regexEnter.IsMatch(content);
        }

        public static string EncryptionHash(int hash)
        {
            hash = Math.Abs(hash);
            if (HashDic.ContainsKey(hash))
            {
                return HashDic[hash];
            }

            throw new ArgumentException("hash must in hashDic");
        }

        private static Dictionary<int, string> _hashDic;
        //public void GetRandom()
        //{
        //    var result = "";
        //    List<string> list = new List<string>();
        //    Random r = new Random();
        //    for (var i = 0; i < 100; )
        //    {
        //        var num = r.Next(676);
        //        char charA = (char)('a' + (num / 26));
        //        char charB = (char)('a' + (num % 26));
        //        var str = charA + "" + charB;
        //        if (!list.Contains(str))
        //        {
        //            var hash = GetMd5String(str + "xuna");
        //            str = str + (char)('a' + hash[0] % 26);
        //            list.Add(str);
        //            i++;
        //        }
        //    }
        //    var ss = string.Join(",", list.ToArray());
        //}
        private const string hashValues =
            "ugy,ffy,zyc,aau,iqw,yvf,elt,get,hef,oky," +
            "yma,dfb,dfb,dge,hua,sic,mgu,slf,hxd,zwy," +
            "qpa,hpv,yky,juv,obx,aoy,zbz,hcd,lez,jzd," +
            "qqx,rzv,tlf,zrw,cjw,yjt,puy,mia,few,akz," +
            "spa,qda,wwy,lpu,duf,lfw,hfx,qrx,pka,paa," +
            "sew,bmv,usv,zbz,gsy,mqy,guf,qex,lyd,ibb," +
            "zuu,xib,vaz,xed,xtx,gmb,jtb,dny,axx,jkc," +
            "cnx,zyc,nhv,hnv,hhw,amt,ktt,muy,fzv,cvc," +
            "tje,ijx,rsy,wdx,rvy,pez,xhe,uja,syw,psw," +
            "roa,zpu,aja,xcy,aqx,vgx,vct,zrw,nwu,xib";

        private static object hashLock = new object();
        public static Dictionary<int, string> HashDic
        {
            get
            {
                if (_hashDic == null)
                {
                    lock (hashLock)
                    {
                        if (_hashDic == null)
                        {
                            _hashDic = new Dictionary<int, string>();
                            var values = hashValues.Split(',');
                            for (var i = 0; i < 100; i++)
                            {
                                _hashDic.Add(i, values[i]);
                            }
                        }
                    }
                }
                return _hashDic;
            }
        }

        //public static string DecryptHash(string hash)
        //{
        //    if (string.IsNullOrWhiteSpace(hash))
        //    {
        //        return hash;
        //    }
        //    return hash;
        //}

        private static Regex hashRegex = new Regex("\\.(" + string.Join("|", hashValues.Split(',')) + "){1}\\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static bool ContainsHashDomain(string domain, out string hash)
        {
            var match = hashRegex.Match(domain);
            hash = match.Success ? match.Value : string.Empty;
            return match.Success;
        }

        public static string TransferSecondDomain(string domain, bool isInternal)
        {
            if (string.IsNullOrWhiteSpace(domain) || domain.IndexOf(".") == -1)
            {
                return domain;
            }
            string hash;
            if (ContainsHashDomain(domain, out hash))
            {
                return domain.Replace(hash, hash + (isInternal ? "internal." : "transfer."));
            }
            return domain.Insert(domain.IndexOf("."), isInternal ? ".internal" : ".transfer");
        }

        public static string RetransferSecondDomain(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                return domain;
            }
            return domain.ToLower().Replace(".internal.", ".").Replace(".transfer.", ".");
        }

        /// <summary>
        /// 获取HostIP信息
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string GetInternetIpAddress(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return "不能获取IP,host为空";
            }

            try
            {
                IPHostEntry hostEntity = Dns.GetHostEntry(host);
                if (hostEntity != null && hostEntity.AddressList != null)
                {
                    return string.Join(",", hostEntity.AddressList.Select(m => m.ToString()).ToArray());
                }
                return "不能获取IP,不能获取主机信息";
            }
            catch (Exception ex)
            {
                return "不能获取IP," + ex.Message;
            }
        }

        public static string ConvertToRelativeProtocol(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                return url.Substring("http:".Length);
            }
            if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return url.Substring("https:".Length);
            }
            //if (url.StartsWith("//", StringComparison.OrdinalIgnoreCase))
            //{
            //    return url;
            //}
            return url;
        }
        /// <summary>
        /// 正则替换所有的http://为//,  addedby jjl
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ConvertHttpProtocalToRelative(string url)
        { 
            Regex reg = new Regex("http://", RegexOptions.IgnoreCase);

            string tempUrl = reg.Replace(url, "//");
             
            return tempUrl;
        }

        public static int MaxCategoryCount()
        {
            return 1000;
        }

        public static int MaxPageCount()
        {
            return 2000;
        }

        public static int MaxHeadFooterCount()
        {
            return 100;
        }
        public static int MaxSystemTemplateCount()
        {
            return 100;
        }
    }
}
