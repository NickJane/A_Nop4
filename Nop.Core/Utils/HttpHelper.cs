using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net.Cache;

namespace Nop.Core.Utils
{
    public static class HttpHelper
    {

        /// <summary>
        /// 使用Get方法获取字符串结果（暂时没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            return HttpGet(url, null, null);
        }


        /// <summary>
        /// 使用Get方法获取字符串结果（暂时没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Encoding encoding)
        {
            return HttpGet(url, null, encoding);
        }

        /// <summary>
        /// 超时3秒 使用Get方法获取字符串结果（暂时没有加入Cookie）,parameters字典里面东西会通过循环在request.Headers中添加
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGet(string url, IDictionary<string, string> parameters = null, Encoding encoding = null)
        {
            Encoding readCoding = encoding ?? Encoding.UTF8;

            Uri requestUri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Timeout = 3000;
            request.UserAgent = "mozilla/4.0 (compatible; msie 6.0; windows 2000)";
            request.Method = "Get";
            request.ContentType = "application/x-www-form-urlencoded";
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var item in parameters)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), readCoding))
                {
                    string content = sr.ReadToEnd();
                    return content;
                }
            }
            catch (WebException ex)
            {
                //若是远程服务器抛出了异常，则捕获并解析
                response = (HttpWebResponse)ex.Response;
                return ex.Message;
            }
            finally
            {
                //释放请求的资源
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postStream">例如Encoding.UTF8.GetBytes(json.Serialize(new { email = "123456@qq.com", password = "111111" }))</param>
        /// <param name="parameters">字典里面东西会通过循环在request.Headers中添加</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpPost(string url, byte[] postStream, IDictionary<string, string> parameters = null, Encoding encoding = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ProtocolVersion = HttpVersion.Version11;
            request.Method = "POST";

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var item in parameters)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            request.ContentType = "application/json;charset=utf-8";
            request.Accept = "application/json";

            request.Timeout = 60 * 2 * 1000; // 同步接口 调用时间2分钟
            request.ServicePoint.Expect100Continue = false;

            HttpWebResponse response = null;
            try
            {
                postStream = postStream ?? new byte[] { };
                request.ContentLength = postStream.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(postStream, 0, postStream.Length);
                requestStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
                        {
                            return myStreamReader.ReadToEnd();
                        }
                    }
                }
            }
            
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                request.Abort();
            }
            return "";
        }
        
        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <returns></returns>
        public static string HttpPost(string url, CookieContainer cookieContainer = null, Dictionary<string, string> formData = null, Encoding encoding = null)
        {
            return HttpPost(url, cookieContainer, formData, encoding, 12000);
        }
        public static string HttpPost(string url, CookieContainer cookieContainer = null, Dictionary<string, string> formData = null, Encoding encoding = null, int timeout = 20)
        {
            string dataString = GetQueryString(formData);
            var formDataBytes = formData == null ? new byte[0] : (encoding == null ? Encoding.UTF8.GetBytes(dataString) : encoding.GetBytes(dataString));
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(formDataBytes, 0, formDataBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置
                string ret = HttpPost(url, cookieContainer, ms, false, encoding, timeout);
                return ret;
            }
        }
        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpPost(string url, CookieContainer cookieContainer = null, string fileName = null, Encoding encoding = null)
        {
            //读取文件
            FileStream fileStream = null;
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open);
            }
            return HttpPost(url, cookieContainer, fileStream, true, encoding);
        }

        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="postStream"></param>
        /// <param name="isFile">postStreams是否是文件流</param>
        /// <returns></returns>
        public static string HttpPost(string url, CookieContainer cookieContainer = null, Stream postStream = null, bool isFile = false, Encoding encoding = null, int timeout = 1200000)
        {
            ServicePointManager.DefaultConnectionLimit = 200;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postStream != null ? postStream.Length : 0;
            request.Timeout = timeout;
            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }
            if (postStream != null)
            {
                //postStream.Position = 0;

                //上传文件流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                postStream.Close();//关闭文件访问
            }
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

                if (cookieContainer != null)
                {
                    response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.GetEncoding("utf-8")))
                    {
                        string retString = myStreamReader.ReadToEnd();
                        return retString;
                    }
                }
            }
            
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (request != null)
                {
                    request.Abort();
                }
            }


        }

        /// <summary>
        /// 组装QueryString的方法
        /// 参数之间用&连接，首位没有符号，如：a=1&b=2&c=3
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        public static string GetQueryString(this Dictionary<string, string> formData)
        {
            if (formData == null || formData.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            var i = 0;
            foreach (var kv in formData)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
                if (i < formData.Count)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.HtmlEncode
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string html)
        {
            return System.Web.HttpUtility.HtmlEncode(html);
        }
        /// <summary>
        /// 封装System.Web.HttpUtility.HtmlDecode
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string html)
        {
            return System.Web.HttpUtility.HtmlDecode(html);
        }
        /// <summary>
        /// 封装System.Web.HttpUtility.UrlEncode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(this string url)
        {
            return System.Web.HttpUtility.UrlEncode(url);
        }
        /// <summary>
        /// 封装System.Web.HttpUtility.UrlDecode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(this string url)
        {
            return System.Web.HttpUtility.UrlDecode(url);
        }
    }
}
