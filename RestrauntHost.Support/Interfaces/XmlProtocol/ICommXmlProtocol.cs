namespace RestaurantHost.Service.Interfaces.XmlProtocol
{

    public interface ICommXmlProtocolService
    {
        string Serialize<T>(T obj);  // 어떤 타입이든 XML로 변환
        T Deserialize<T>(string xml); // 어떤 타입이든 xml로부터 복원
        void SaveToFile<T>(T obj, string filePath); // 제네릭 타입 T의 객체를
                                                    // XML 문자열로 직렬화(Serialize) 한 다음,
                                                    // 그것을 파일로 저장.
        T LoadFromFile<T>(string filePath);
        string FormatXml(string xml);// optional, pretty-print
    }
}