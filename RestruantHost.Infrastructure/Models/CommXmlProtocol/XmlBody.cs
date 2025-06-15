using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestaurantHost.Support.Models.CommXmlProtocol
{  
    #region S2F2 메뉴판 요청에 대한 응답
    public class S2F2Data
    {
        [XmlArray("FOOD_INFO_LIST")]
        [XmlArrayItem("FOOD_INFO")]
        public List<FoodInfoType1> FoodInfoList { get; set; }
    }

    public class FoodInfoType1
    {
        [XmlElement("FOOD")]
        public string Food { get; set; }

        [XmlElement("PRICE")]
        public int Price { get; set; }
    }
    #endregion

    #region S3F2 SVR->CLI  주문요청(CEID: 100)에 대한 응답 || 결제 요청(CEID: 101)에 대한 응답     S3F1C100, C101  CLI->SVR  
    public class S3F2Data { }
    #endregion 

    #region S4F2 곧 나와요 ~!
    public class S4F2
    {
        [XmlElement("REMAIN_TIME")]
        DateTime RemainTime;
    }
    #endregion

    #region S5F1 조리완료에 따른 음식제공 
    public class S3F1Data
    {
        [XmlArray("FOOD_INFO_LIST")]
        [XmlArrayItem("FOOD_INFO")]
        public List<FoodInfoType2> FoodInfoList { get; set; }
    }

    public class FoodInfoType2
    {
        [XmlElement("FOOD")]
        public string Food { get; set; }

        [XmlElement("COUNT")]
        public int Count { get; set; }
    }
    #endregion

    #region S7F1 만석에 따른 대기 요청
    public class S7F1Data { }
    #endregion

    #region S8F1 자리 빔에 따른 출입 요청
    public class S8F1Data{ }
    #endregion
}
