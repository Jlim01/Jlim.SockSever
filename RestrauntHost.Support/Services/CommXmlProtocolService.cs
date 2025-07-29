using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using RestaurantHost.Service.Helpers;
using RestaurantHost.Service.Interfaces.XmlProtocol;

namespace RestaurantHost.Service.Services
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
                Encoding = new UTF8Encoding(false), // BOM 없이,
                OmitXmlDeclaration = false
            };

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty); // 네임스페이스 제거

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, settings);

            //ns 인자 사용에 따른, <MESSAGE xmlns:xsd="http://www.w3.org/2001/XMLSchema"> 의 xmlns(네임스페이스) 삭제.
            serializer.Serialize(xmlWriter, obj, ns); 
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
