using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using RestaurantHost.Support.Helpers;
using RestaurantHost.Support.Interfaces.XmlProtocol;

namespace RestaurantHost.Support.Services
{
    public class CommXmlProtocolService : ICommXmlProtocolService
    {
        public T Deserialize<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var stringReader = new StringReader(xml);
            return (T)serializer.Deserialize(stringReader);
        }

        public string Serialize<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false
            };

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, settings);
            serializer.Serialize(xmlWriter, obj);
            return stringWriter.ToString();
        }

        public T LoadFromFile<T>(string filePath)
        {
            var xml = File.ReadAllText(filePath);
            return Deserialize<T>(xml);
        }

        public void SaveToFile<T>(T obj, string filePath)
        {
            var xml = Serialize(obj);
            File.WriteAllText(filePath, xml);
        }
        public string FormatXml(string xml)
        {
            return XmlFormatter.Format(xml);
        }
    }
}
