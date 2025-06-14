using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestaurantHost.Support.Models.CommXmlProtocol
{
    public class CommonBodyBase
    {
        [XmlElement("Data")]
        public string Data { get; set; }
    }

    public class S8F1 : CommonBodyBase { }  // svr->cli
   
    #region S2F2
    public class S2F2 : CommonBodyBase
    {
        [XmlElement("DATA")]
        public S2F2Data Data { get; set; }
    }

    public class S2F2Data
    {
        [XmlArray("FOOD_INFO_LIST")]
        [XmlArrayItem("FOOD_INFO")]
        public List<FoodInfo> FoodInfoList { get; set; }
    }

    public class FoodInfo
    {
        [XmlElement("FOOD")]
        public string Food { get; set; }

        [XmlElement("PRICE")]
        public int Price { get; set; }
    }
    #endregion


}
