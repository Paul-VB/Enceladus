using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Enceladus.Core.Utils
{
    public static class XmlHelper
    {
        #region Deserialize
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

        public static T Deserialize<T>(XDocument document)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(document.CreateReader())!;
        }
        #endregion

        #region Serialize

        private static readonly XmlWriterSettings DefaultXmlWriterSettings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "\t"
        };
        public static string Serialize(object obj, XmlWriterSettings? settings = null)
        {
            var serializer = new XmlSerializer(obj.GetType());
            settings ??= DefaultXmlWriterSettings;

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
            settings ??= DefaultXmlWriterSettings;

            using (var writer = XmlWriter.Create(filePath, settings))
            {
                serializer.Serialize(writer, obj);
            }
        }
        #endregion

        #region Merge


        //left join
        public static XDocument Merge(XDocument baseDoc, XDocument overrideDoc)
        {
            var mergedDoc = new XDocument(baseDoc);

            MergeElements(mergedDoc.Root, overrideDoc.Root);

            return mergedDoc;
        }

        private static void MergeElements(XElement baseElement, XElement overrideElement)
        {
            if (overrideElement == null) return;

            foreach (var baseChild in baseElement.Elements())
            {
                var overrideChild = overrideElement.Elements(baseChild.Name).FirstOrDefault();
                if (overrideChild == null) continue;

                if (baseChild.HasElements)
                {
                    MergeElements(baseChild, overrideChild);
                }
                else
                {
                    baseChild.Value = overrideChild.Value;
                }
            }
        }
        #endregion
    }
}
