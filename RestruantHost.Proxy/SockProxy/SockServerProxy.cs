using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;

namespace RestaurantHost.Proxy.SockProxy
{
    //todo:
    // 서버 환경 구축.
    //xml 불러서 문자열 파싱 
    // proxy 문자열 receive 역할
    // service에게 이벤트로 message 넘겨주기  service는 받은 메시지 비즈니스 로직 처리. service가 send 역할도 함.
    // 
    public class SockServerProxy
    {
        //public delegate void AsyncSockReceivedEventHandler(int moduleID, SockMessage message);
        //public event AsyncSockReceivedEventHandler DataReceivedEventHandler;
        //Action : 반환 값 없는 이벤트핸들러로 상기 내용을 한 줄로 만들 수 있다.
        public event Action<int, SockMessage>? DataRecivedEventHandler;
        public void test()
        {
            DataRecivedEventHandler?.Invoke(1, new SockMessage(1, "Test"));
        }
        public SockServerProxy()
        {
            //todo: 초기 configure 등록할거면 등록하기. 예. 데이터 송수신에 대한 Cancellationtoken 시간. 
            // port번호
            // 통신방법 : tcp/ip 소켓통신, 시리얼통신...
            //Listen 대기시간.
            //메시지 방식 구조체 , mes, xml ...

            // db로 하는게 xml이나 json으로 configure 설정보다 나을듯.
            //...
            
        }
        static async Task CreateSockAsyncServer()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, 11000));
            listener.Listen(10000);


            Debug.WriteLine("Waiting for client...");
            Socket client = await listener.AcceptAsync();
            Debug.WriteLine("Client connected.");

            var receiveTask = ReceiveLoopAsync(client);


            client.Close();
            listener.Close();
        }
        static async Task ReceiveLoopAsync(Socket client)
        {
            byte[] buffer = new byte[8192];
            while (true)
            {
                int received;
                try
                {
                    received = await client.ReceiveAsync(buffer, SocketFlags.None);
                    if (received == 0) break;
                    string rcvData = Encoding.UTF8.GetString(buffer, 0, received);
                    var header = Regex.Match(rcvData, @"<HEADER>(.*?)</HEADER>");
                    if (header.Success)
                    {
                        Debug.WriteLine($"Received Header: {header.Groups[1].Value}");
                        
                    }
                    else
                    {
                        Debug.WriteLine("Header not found in received data.");
                    }
                }
                catch
                {
                    Debug.WriteLine("Disconnected during receive.");

                }

                //헤더 구분 하기.
                //메시지 Service로 이관


            }
        }

        public void SendMessage(SockMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
