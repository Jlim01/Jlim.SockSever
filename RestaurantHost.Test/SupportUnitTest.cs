using RestaurantHost.Support.Helpers;
using RestaurantHost.Support.Models.CommXmlProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHost.Test
{


    public class CommXmlProtocolServiceTests
    {
        [Fact]
        public void Serialize_Should_Contain_PropertyNames()
        {
            // Arrange
            var service = new CommXmlProtocolService();
            var obj = new { Name = "홍길동", Age = 30 };

            // Act
            var xml = service.Serialize(obj);

            // Assert
            Assert.Contains("홍길동", xml);
            Assert.Contains("Age", xml);
        }
    }
}
