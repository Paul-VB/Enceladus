using System.Xml;
using System.Xml.Serialization;

namespace Enceladus.Core.Utils
{
    public static class XmlSerializerHelper
    {
        private static readonly XmlWriterSettings DefaultSettings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "\t"
        };

        public static T Deserialize<T>(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream)!;
        }

        public static T Deserialize<T>(StreamReader streamReader)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(streamReader)!;
        }

        public static T Deserialize<T>(string str)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new StringReader(str))!;
        }

        public static string Serialize(object obj, XmlWriterSettings? settings = null)
        {
            var serializer = new XmlSerializer(obj.GetType());
            settings ??= DefaultSettings;

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(xmlWriter, obj);
                return stringWriter.ToString();
            }
        }

        public static void SerializeToFile(object obj, string filePath, XmlWriterSettings? settings = null)
        {
            var serializer = new XmlSerializer(obj.GetType());
            settings ??= DefaultSettings;

            using (var writer = XmlWriter.Create(filePath, settings))
            {
                serializer.Serialize(writer, obj);
            }
        }
    }
}
