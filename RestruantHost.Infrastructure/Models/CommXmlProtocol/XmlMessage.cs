using RestaurantHost.Support.Models.CommXmlProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestaurantHost.Infrastructure.Models.CommXmlProtocol
{
    [XmlRoot("MESSAGE")]
    public class XmlMessage<T>
    {
        [XmlElement("HEADER")]
        public XmlHeader Header { get; set; }

        [XmlElement("DATA")]
        public T Body { get; set; }
    }
}
