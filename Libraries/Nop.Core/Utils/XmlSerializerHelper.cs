using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Nop.Core.Utils
{
    public static class XmlSerializerHelper
    {
        public static string Serialize<T>(T item)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                using (XmlTextWriter textWriter = new XmlTextWriter(memStream, Encoding.UTF8))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(textWriter, item);


                }
                return Encoding.UTF8.GetString(memStream.ToArray());
            }
        }

        public static string SerializeWithoutXmlns<T>(T item)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                using (XmlTextWriter textWriter = new XmlTextWriter(memStream, Encoding.UTF8))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(textWriter, item, ns);


                }
                return Encoding.UTF8.GetString(memStream.ToArray());
            }
        }
        /// <summary>
        /// xml序列化，去掉xml声明和命名空间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string PureSerialize<T>(T item)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Indent = false,
                    OmitXmlDeclaration = true,
                    Encoding = Encoding.UTF8
                };
                using (XmlWriter textWriter = XmlWriter.Create(memStream, settings))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(textWriter, item, ns);
                }
                return Encoding.UTF8.GetString(memStream.ToArray());
            }
        }

        public static object Deserialize(Type type, string xmlString)
        {
            if (string.IsNullOrWhiteSpace(xmlString))
                return null;

            using (MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(memStream);

            }
        }
        public static T Deserialize<T>(string xmlString)
        {
            if (string.IsNullOrWhiteSpace(xmlString))
                return default(T);

            using (MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(memStream);

            }
        }
        public static T Deserialize<T>(string xmlString, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(xmlString))
                return default(T);

            using (MemoryStream memStream = new MemoryStream((encoding ?? Encoding.UTF8).GetBytes(xmlString)))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(memStream);

            }
        }
        public static T Deserialize<T>(Stream xmlStream)
        {
            if (xmlStream == null)
                return default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(xmlStream);
        }
        public static object Deserialize(Type type, Stream xmlStream)
        {
            if (xmlStream == null)
                return null;
            XmlSerializer serializer = new XmlSerializer(type);
            return serializer.Deserialize(xmlStream);
        }
    }
}
