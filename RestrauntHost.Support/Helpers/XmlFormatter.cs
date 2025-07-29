using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RestaurantHost.Service.Helpers
{
    public static class XmlFormatter
    {
        public static string Format(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            using var stringWriter = new StringWriter();
            using var xmlTextWriter = new XmlTextWriter(stringWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 2
            };

            doc.Save(xmlTextWriter);
            return stringWriter.ToString();
        }
    }
}
