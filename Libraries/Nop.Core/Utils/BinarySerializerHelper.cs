using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Nop.Core.Utils
{

    public static class BinarySerializerHelper
    {
        public static byte[] Serialize<T>(T item)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, item);
                return stream.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            using (MemoryStream memStream = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(memStream);
            }
        }
    }

}
