using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHost.Support.Interfaces;

namespace RestaurantHost.Support.Helpers
{
    public class CommXmlProtocolService : ICommXmlProtocolService
    {
        public CommXmlProtocolService()
        {
            
        }
        public T Deserialize<T>(string xml)
        {
            throw new NotImplementedException();
        }

        public string FormatXml(string xml)
        {
            throw new NotImplementedException();
        }

        public T LoadFromFile<T>(string filePath)
        {
            throw new NotImplementedException();
        }

        public void SaveToFile<T>(T obj, string filePath)
        {
            throw new NotImplementedException();
        }

        public string Serialize<T>(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
