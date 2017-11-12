using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Xml;
using System.IO.Compression;

namespace Nop.Core
{
    static public class Utilities
    {
        private delegate T ParseHandler<T>(string source);
        private delegate bool TryParseHandler<T>(string source, out T value);

        static private Dictionary<Type, Delegate> parseHandlerPool = new Dictionary<Type, Delegate>();
        static private ReaderWriterLockSlim _parseHandlerPoolLocker = new ReaderWriterLockSlim();

        static public T SafeCastFromString<T>(string source, T defaultValue)
        {
            if (defaultValue is string)
            {
                return (T)(object)source;
            }
            T result;
            try
            {
                if (typeof(T).IsEnum)
                {
                    return (T)System.Enum.Parse(typeof(T), source, true);
                }
                else
                {
                    Delegate parseHandler = RetrieveHandler<T>();
                    TryParseHandler<T> tryParser = parseHandler as TryParseHandler<T>;

                    if (tryParser != null)
                    {
                        if (tryParser(source, out result) == false)
                        {
                            result = defaultValue;
                        }
                    }
                    else
                    {
                        ParseHandler<T> parser = parseHandler as ParseHandler<T>;

                        if (parser != null)
                        {
                            result = parser(source);
                        }
                        else
                        {
                            result = defaultValue;
                        }
                    }
                }
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        static public T SafeCastDatabaseObject<T>(object source, T defaultValue)
        {
            if (source == null || source is DBNull)
            {
                return defaultValue;
            }
            else
            {
                return (T)source;
            }
        }

        static public T SafeCastObject<T>(object source, T defaultValue)
        {
            if (source != null)
            {
                return (T)source;
            }
            else
            {
                return defaultValue;
            }
        }

        static private Delegate RetrieveHandler<T>()
        {
            Type t = typeof(T);
            Delegate result;

            // read from pool
            _parseHandlerPoolLocker.EnterReadLock();
            try
            {
                if (parseHandlerPool.TryGetValue(t, out result))
                {
                    return result;
                }
            }
            finally
            {
                _parseHandlerPoolLocker.ExitReadLock();
            }

            _parseHandlerPoolLocker.EnterUpgradeableReadLock();
            try
            {
                if (parseHandlerPool.TryGetValue(t, out result) == false) // check again
                {
                    // 重新计算
                    result = TryGetTryParseHandler<T>(t);
                    if (result == null)
                    {
                        result = TryGetParser<T>(t);
                    }
                    else
                    {
                        result = null;
                    }

                    // write it
                    _parseHandlerPoolLocker.EnterWriteLock();
                    try
                    {
                        parseHandlerPool.Add(t, result);
                    }
                    finally
                    {
                        _parseHandlerPoolLocker.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _parseHandlerPoolLocker.ExitUpgradeableReadLock();
            }

            return result;
        }

        static private TryParseHandler<T> TryGetTryParseHandler<T>(Type t)
        {
            MemberInfo[] members;
            members = t.GetMember("TryParse", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            foreach (MethodInfo mi in members)
            {
                ParameterInfo[] parameters = mi.GetParameters();
                if (parameters[0].ParameterType == typeof(string) &&
                    parameters[1].ParameterType.FullName == typeof(T).FullName + "&")
                {
                    TryParseHandler<T> parseHandler = (TryParseHandler<T>)Delegate.CreateDelegate(typeof(TryParseHandler<T>), mi);
                    return (TryParseHandler<T>)parseHandler;
                }
            }
            return null;
        }

        static private ParseHandler<T> TryGetParser<T>(Type t)
        {
            MethodInfo mi;
            mi = t.GetMethod("Parse", new Type[] { typeof(string) });
            if (mi == null)
            {
                return null;
            }
            else
            {
                ParseHandler<T> parseHandler = (ParseHandler<T>)Delegate.CreateDelegate(typeof(ParseHandler<T>), mi);
                return (ParseHandler<T>)parseHandler;
            }
        }

        public static string ShortDateTimeString(DateTime dateTime)
        {
            DateTime today = DateTime.Today;
            DateTime date = dateTime.Date;
            TimeSpan span = today - date;

            if (date.Year != today.Year || date.Month != today.Month)
            {
                return dateTime.ToString("yy-MM");
            }
            else if (date.Day != today.Day)
            {
                return dateTime.ToString("dd日");
            }
            else // if (date.Day == today.Day)
            {
                return dateTime.ToString("HH:mm");
            }
        }

        /// <summary>
        /// 从一个XML节点获取一个属性的字符串
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="name">属性名称</param>
        /// <returns>返回的字符串</returns>
        public static string SafeGetAttribute(XmlNode node, string name)
        {
            return SafeGetAttribute(node, name, false);
        }

        /// <summary>
        /// 从一个XML节点获取一个属性的字符串
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="name">属性名称</param>
        /// <param name="throwExptionWhenAttributeNull">如果属性不存在是否抛出异常</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static string SafeGetAttribute(XmlNode node, string name, bool throwExptionWhenAttributeNull)
        {
            XmlAttribute attribute = node.Attributes[name];

            if (attribute != null)
            {
                return attribute.Value.Trim();
            }
            else
            {
                if (throwExptionWhenAttributeNull)
                {
                    throw new ArgumentNullException(string.Format("属性{0}不存在。", name));
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string MapPath(string virtualFileName)
        {
            string result = System.Web.Hosting.HostingEnvironment.MapPath(virtualFileName);
            if (result == null)
            {
                if (virtualFileName.StartsWith("~/"))
                {
                    result = virtualFileName.Substring(2);
                }
                else if (virtualFileName.StartsWith("/"))
                {
                    result = virtualFileName.Substring(1);
                }
                else
                {
                    result = virtualFileName;
                }
                result = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, result);
            }
            return result;
        }

        public static string GetFileExtName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;
            char[] chars = { '.', '/', '\\' };
            int index = fileName.LastIndexOfAny(chars);
            if (index < 0)
                return fileName;

            string extName = fileName.Substring(index + 1);
            return extName;
        }


        /// <summary>  
        /// GZip压缩  
        /// </summary>  
        /// <param name="rawData"></param>  
        /// <returns></returns>  
        public static MemoryStream Compress(byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            ms.Position = 0;
            return ms;
        }
        public static long ZipCompress(Stream stream, string zipFileName)
        {
            using (FileStream compressedFileStream = File.Create(zipFileName))
            {
                using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                   CompressionLevel.Fastest))
                {
                    stream.CopyTo(compressionStream);
                    return compressedFileStream.Length;
                }
            }
        }
        public static void ZipDecompress(Stream zipStream,string unZipFileName)
        {
            using (FileStream decompressedFileStream = File.Create(unZipFileName))
            {
                using (GZipStream decompressionStream = new GZipStream(zipStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(decompressedFileStream);
                }
            }
        }

        /// <summary>  
        /// ZIP解压  
        /// </summary>  
        /// <param name="zippedData"></param>  
        /// <returns></returns>  
        public static MemoryStream Decompress(byte[] zippedData)
        {
            MemoryStream ms = new MemoryStream(zippedData);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            outBuffer.Position = 0;
            return outBuffer;
        }


        /// <summary>
        /// 将 Stream 转成 byte[]  
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始   
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将 byte[] 转成 Stream  
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }


        public static int CopyStream(Stream Src, Stream Trg)
        {
            return CopyStream(Src, Trg, true);
        }

        public static int CopyStream(Stream Src, Stream Trg, bool setSrcPositionToZero)
        {
            if (setSrcPositionToZero)
            {
                Src.Position = 0;
            }

            MemoryStream memSource = Src as MemoryStream;
            if (memSource != null)
            {
                memSource.WriteTo(Trg);
                return (int)(memSource.Length & int.MaxValue);
            }
            else
            {
                byte[] buff = new byte[10240];
                int totalLen = 0;
                int len = Src.Read(buff, 0, buff.Length);
                while (len > 0)
                {
                    Trg.Write(buff, 0, len);
                    totalLen += len;
                    len = Src.Read(buff, 0, buff.Length);
                }
                return totalLen;
            }
        }
    }
}
