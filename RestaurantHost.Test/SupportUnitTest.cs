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
            OutPut.WriteLine(test); // 결과는 동일함. Test모드에선 Format전이나 후나 동일하게 결과 출력.
            // Assert
            Assert.Contains("홍길동", xml);
            Assert.Contains("Age", xml);
        }
    }
}
