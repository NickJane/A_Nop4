using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Nop.Core
{
    public static class Extensions
    {
        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }

        public static string ElText(this XmlNode node, string elName)
        {
            return node.SelectSingleNode(elName).InnerText;
        }

        public static string ToQueryString(this Dictionary<string, string> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return "";
            }

            var sb = new StringBuilder();

            var i = 0;
            foreach (var kv in dictionary)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
                if (i < dictionary.Count)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue)
            where TInput : class
        {
            return o == null ? failureValue : evaluator(o);
        }
    }

    public static class ValidatorExtensions
    {
        #region  验证输入字符串为数字
        /// <summary>
        /// 验证输入字符串为数字
        /// </summary>
        /// <param name="source">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsNumber(this string source)
        {
            return Regex.IsMatch(source, "^([0]|([1-9]+\\d{0,}?))(.[\\d]+)?$");
        }

        /// <summary>
        /// 验证输入字符串为整数
        /// </summary>
        /// <param name="source">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsInt(this string source)
        {
            return Regex.IsMatch(source, "^([1-9]\\d{0,8}|0)$");
        }
        #endregion


        /// <summary>
        /// 判断用户输入是否为日期
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks>
        /// 可判断格式如下（其中-可替换为/，不影响验证)
        /// YYYY | YYYY-MM | YYYY-MM-DD | YYYY-MM-DD HH:MM:SS | YYYY-MM-DD HH:MM:SS.FFF
        /// </remarks>
        public static bool IsDateTime(this string source)
        {
            if (null == source)
            {
                return false;
            }
            string regexDate = @"[1-2]{1}[0-9]{3}((-|\/|\.){1}(([0]?[1-9]{1})|(1[0-2]{1}))((-|\/|\.){1}((([0]?[1-9]{1})|([1-2]{1}[0-9]{1})|(3[0-1]{1})))( (([0-1]{1}[0-9]{1})|2[0-3]{1}):([0-5]{1}[0-9]{1}):([0-5]{1}[0-9]{1})(\.[0-9]{3})?)?)?)?$";
            if (Regex.IsMatch(source, regexDate))
            {
                //以下各月份日期验证，保证验证的完整性
                int _IndexY = -1;
                int _IndexM = -1;
                int _IndexD = -1;
                if (-1 != (_IndexY = source.IndexOf("-")))
                {
                    _IndexM = source.IndexOf("-", _IndexY + 1);
                    _IndexD = source.IndexOf(":");
                }
                else
                {
                    _IndexY = source.IndexOf("/");
                    _IndexM = source.IndexOf("/", _IndexY + 1);
                    _IndexD = source.IndexOf(":");
                }
                //不包含日期部分，直接返回true
                if (-1 == _IndexM)
                    return true;
                if (-1 == _IndexD)
                {
                    _IndexD = source.Length + 3;
                }
                int iYear = Convert.ToInt32(source.Substring(0, _IndexY));
                int iMonth = Convert.ToInt32(source.Substring(_IndexY + 1, _IndexM - _IndexY - 1));
                int iDate = Convert.ToInt32(source.Substring(_IndexM + 1, _IndexD - _IndexM - 4));
                //判断月份日期
                if ((iMonth < 8 && 1 == iMonth % 2) || (iMonth > 8 && 0 == iMonth % 2))
                {
                    if (iDate < 32)
                        return true;
                }
                else
                {
                    if (iMonth != 2)
                    {
                        if (iDate < 31)
                            return true;
                    }
                    else
                    {
                        //闰年
                        if ((0 == iYear % 400) || (0 == iYear % 4 && 0 < iYear % 100))
                        {
                            if (iDate < 30)
                                return true;
                        }
                        else
                        {
                            if (iDate < 29)
                                return true;
                        }
                    }
                }
            }
            return false;
        }


        #region 验证手机号
        /// <summary>
        /// 验证输入字符串为18位的手机号码
        /// </summary>
        /// <param name="source">输入的字符</param>
        /// <returns></returns>
        public static bool IsMobile(string source)
        {
            return Regex.IsMatch(source, @"^1[0123456789]\d{9}$", RegexOptions.IgnoreCase);
        }

        #endregion

        /// <summary>
        /// 验证输入字符串为电话号码
        /// </summary>
        /// <param name="source">输入的字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsPhone(this string source)
        {
            return Regex.IsMatch(source, @"(^(\d{2,4}[-_－—]?)?\d{3,8}([-_－—]?\d{3,8})?([-_－—]?\d{1,7})?$)|(^0?1[35]\d{9}$)");
        }


        /// <summary>
        /// 验证是否是有效邮箱地址
        /// </summary>
        /// <param name="source">输入的字符</param>
        /// <returns></returns>
        public static bool IsEmail(this string source)
        {
            return Regex.IsMatch(source, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        /// <summary>
        /// 验证是否是有效传真号码
        /// </summary>
        /// <param name="source">输入的字符</param>
        /// <returns></returns>
        public static bool IsFax(this string source)
        {
            return Regex.IsMatch(source, @"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$");
        }
        /// <summary>
        /// 验证是否只含有汉字
        /// </summary>
        /// <param name="source">输入的字符</param>
        /// <returns></returns>
        public static bool IsOnllyChinese(this string source)
        {
            return Regex.IsMatch(source, @"^[\u4e00-\u9fa5]+$");
        }
        public static bool IsIp(this string source)
        {
            if (Regex.IsMatch(source, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {
                string[] ips = source.Split('.');
                if (ips.Length == 4 || ips.Length == 6)
                {
                    if (Int32.Parse(ips[0]) < 256 && Int32.Parse(ips[1]) < 256 && Int32.Parse(ips[2]) < 256 && Int32.Parse(ips[3]) < 256)
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            else
                return false;
        }

        public static bool IsDomain(this string source)
        {
            return Regex.IsMatch(source, @"^([a-z0-9\-\u4E00-\u9FA5]*[\.])+([a-z\u4E00-\u9FA5]{2,10})$", RegexOptions.IgnoreCase);
        }

        public static bool IsSeoFriendName(this string source)
        {
            return Regex.IsMatch(source, @"^\w*[a-zA-Z]+\w*$", RegexOptions.IgnoreCase);
        }
    }

    public static class EncodingExtensions
    {
        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Base64Encode(this string source)
        {
            var barray = Encoding.Default.GetBytes(source);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Base64Decode(this string source)
        {
            try
            {
                var barray = Convert.FromBase64String(source);
                return Encoding.Default.GetString(barray);
            }
            catch
            {
                return source;
            }
        }

    }

    public static class DataParseExtensions
    {
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(this object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression, defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(this string expression, bool defValue)
        {
            if (expression != null)
            {
                if (String.Compare(expression, "true", StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
                else if (String.Compare(expression, "false", StringComparison.OrdinalIgnoreCase) == 0)
                    return false;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(this object expression)
        {
            return ObjectToInt(expression, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(this object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型,转换失败返回0
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(this string expression)
        {
            return StrToInt(expression, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(expression, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(expression, defValue));
        }

        /// <summary>
        /// 将对象转换为Int64类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static Int64 StrToInt64(string expression, Int64 defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 20 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            Int64 rv;
            if (Int64.TryParse(expression, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(expression, defValue));
        }

        /// <summary>
        /// 将对象转换为Int64类型,转换失败返回0
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static Int64 StrToInt64(this string expression)
        {
            return StrToInt64(expression, 0);
        }


        public static Int64 ObjectToInt64(this object expression)
        {
            return ObjectToInt64(expression, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static Int64 ObjectToInt64(this object expression, Int64 defValue)
        {
            if (expression != null)
                return StrToInt64(expression.ToString(), defValue);

            return defValue;
        }
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(this object expression, float defValue)
        {
            if ((expression == null))
                return defValue;

            return StrToFloat(expression.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(this object expression, float defValue)
        {
            if ((expression == null))
                return defValue;

            return StrToFloat(expression.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(this object expression)
        {
            return ObjectToFloat(expression.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(this string expression)
        {
            if ((expression == null))
                return 0;

            return StrToFloat(expression.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(this string expression, float defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            float intValue = defValue;
            {
                bool isFloat = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (isFloat)
                    float.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(this string expression, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                DateTime dateTime;
                if (DateTime.TryParse(expression, out dateTime))
                    return dateTime;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(this string expression)
        {
            return StrToDateTime(expression, DateTime.MinValue);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="expression">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(this object expression)
        {
            return StrToDateTime(expression.ToString());
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="expression">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(this object expression, DateTime defValue)
        {
            return StrToDateTime(expression.ToString(), defValue);
        }

        public static int LongToInt(this long expression)
        {
            if (expression >= int.MaxValue)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(expression);
            }
        }

        public static int DateTimeToTimeStamp(this DateTime datetime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(datetime - startTime).TotalSeconds;
        }
    }

    public static class DictionaryDataSourceExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, TValue defaultValue = default(TValue))
        {
            TValue value;
            if (self.TryGetValue(key, out value))
            {
                return value;
            }
            return defaultValue;
        }

        public static bool EqualValueOrDefault(this Dictionary<string, string> self, string key, string equalValue, string defaultValue = null)
        {
            return self.GetValueOrDefault(key, defaultValue) == equalValue;
        }

        public static string GetValueOrDefault(this Dictionary<string, string> self, string key, string defaultValue, params string[] allowValues)
        {
            string value;
            if (self.TryGetValue(key, out value))
            {
                if (allowValues == null || allowValues.Length == 0 || allowValues.Contains(value))
                    return value;
            }
            return defaultValue;
        }
        /// <summary>
        /// 赋值为可选默认值，
        /// 如果可选默认值为空，则直接赋值
        /// 如果可选默认值不为空，则对值进行校验，校验不通过则使用第一个默认值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="allowValues"></param>
        public static void SetValueOrDefault(this Dictionary<string, string> self, string key, string value, params string[] allowValues)
        {
            if (allowValues != null && allowValues.Length > 0 && !allowValues.Contains(value))
            {
                value = allowValues[0];
            }
            self[key] = value;
        }
        
    }
}
