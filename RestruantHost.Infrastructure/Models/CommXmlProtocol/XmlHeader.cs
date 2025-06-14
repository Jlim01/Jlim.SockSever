using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestaurantHost.Support.Models.CommXmlProtocol
{
    public class XmlHeader
    {
        [XmlElement("LINE_TABLE")]
        public string TableName {  get; set; }
        [XmlElement("FROM")]
        public string From { get; set; }
        [XmlElement("TO")]
        public string To { get; set; }
        [XmlElement("COMMAND")]
        public string Cmd { get; set; }
    }
}


/*
 [XmlRoot("Message")]
public class XmlMessage<T> where T : CommonBodyBase
{
    [XmlElement("Header")]
    public XmlHeader Header { get; set; }

    [XmlElement("Body")]
    public T Body { get; set; }
} 
 */