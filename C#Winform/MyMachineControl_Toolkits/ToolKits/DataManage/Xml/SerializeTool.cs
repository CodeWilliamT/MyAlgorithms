using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;

namespace ToolKits.DataManage.Xml
{
    //需要引用System.Runtime.Serialization.Formatters.Soap.dll
    public class SerializeTool<T>
    {
        public static T SoapFormatterDeserialize(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                SoapFormatter formatter = new SoapFormatter();
                return (T) formatter.Deserialize(stream);
            }
        }

        public static byte[] SoapFormatterSerialize(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new SoapFormatter().Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        public static T XmlSerializerDeserialize(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T) serializer.Deserialize(stream);
            }
        }

        public static byte[] XmlSerializerSerialize(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new XmlSerializer(typeof(T)).Serialize((Stream) stream, obj);
                return stream.ToArray();
            }
        }
    }
}

