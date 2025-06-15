using Microsoft.VisualStudio.TestPlatform.Utilities;
using RestaurantHost.Infrastructure.Models.CommXmlProtocol;
using RestaurantHost.Support.Models.CommXmlProtocol;
using RestaurantHost.Support.Services;
using Xunit.Abstractions;

namespace RestaurantHost.Test
{

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class CommXmlProtocolServiceTests
    {
        private readonly ITestOutputHelper OutPut;

        public CommXmlProtocolServiceTests(ITestOutputHelper outPut)
        {
            OutPut = outPut;
        }

        [Fact]
        public void Serialize_Should_Contain_PropertyNames()
        {
            // Arrange
            var service = new CommXmlProtocolService();
            var person = new Person { Name = "홍길동", Age = 30 };

            // Act
            var xml = service.Serialize(person);
            var test = service.FormatXml(xml);
            OutPut.WriteLine(xml); // Test Explorer에서 output으로 출력
            //OutPut.WriteLine(test); // 결과는 동일함. Test모드에선 Format전이나 후나 동일하게 결과 출력.
            // Assert
            Assert.Contains("홍길동", xml);
            Assert.Contains("Age", xml);
        }

        [Fact]
        public void Deserialize_Should_Recover_Person()
        {
            var service = new CommXmlProtocolService();
            var xml = @"<?xml version=""1.0""?>
                        <Person>
                          <Name>홍길동</Name>
                          <Age>30</Age>
                        </Person>";

            var person = service.Deserialize<Person>(xml);

            Assert.Equal("홍길동", person.Name);
            Assert.Equal(30, person.Age);
        }

        [Fact]
        public void FormatXml_Should_Indent_Xml()
        {
            var service = new CommXmlProtocolService();
            var minifiedXml = "<root><item>value</item></root>";
            OutPut.WriteLine(minifiedXml);
            OutPut.WriteLine("");
            var formatted = service.FormatXml(minifiedXml);
            OutPut.WriteLine(formatted);

            Assert.Contains("\n", formatted); // 줄바꿈이 있는지 확인
            Assert.Contains("<item>value</item>", formatted);
        }

        [Fact]
        public void Test_Should_Merge_XMLMsg()
        {
            var service = new CommXmlProtocolService();

            var s8F1Data = new S8F1Data { };
            var s8F1Msg = new XmlMessage<S8F1Data>
            {
                Header = new XmlHeader { TableName = "1", From = "Svr", To = "Cli", Cmd = "S8F1" },
                Body = s8F1Data
            };
            string s8F1MsgXml = service.Serialize(s8F1Msg);

            OutPut.WriteLine(s8F1MsgXml);
            Assert.Contains("<DATA />", s8F1MsgXml);  // 내용 없으면 <DATA /> 이렇게 나온다. 빈문자열 " " 이라도 넣어야 <DATA> </DATA> 로 나옴

            OutPut.WriteLine("");

            var s2F2Data = new S2F2Data
            {
                FoodInfoList = new List<FoodInfoType1>
                {
                    new FoodInfoType1 { Food = "짜장면", Price = 5000 },
                    new FoodInfoType1 { Food = "짬뽕", Price = 6000 }
                }
            };
            var s2F2Msg = new XmlMessage<S2F2Data>
            {
                Header = new XmlHeader { TableName = "1", From="Svr", To= "Cli", Cmd = "S2F2"},
                Body = s2F2Data
            };

            
            string s2F2MsgXml = service.Serialize(s2F2Msg);

            OutPut.WriteLine(s2F2MsgXml);
            Assert.Contains("<FOOD>짜장면</FOOD>", s2F2MsgXml);
            Assert.Contains("<PRICE>6000</PRICE>", s2F2MsgXml);
        }
    }
}
