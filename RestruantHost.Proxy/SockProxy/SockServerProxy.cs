using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace RestaurantHost.Proxy.SockProxy
{
    public class SockServerProxy
    {
        public event Action<int, SockMessage>? DataRecivedEventHandler;

        private Socket? _listener;
        private readonly ConcurrentDictionary<int, Socket> _clients = new();
        private int _nextClientId = 0;

        public SockServerProxy()
        {
            _ = CreateSockAsyncServer(); // fire-and-forget

            //포트번호 configure로 불러올수있음.
            //cancellation token 취소 시간 또한 configure 설정 가능
        }
        public void test()
        {
            DataRecivedEventHandler.Invoke(0, new SockMessage(1, "S1F1")); // sample test
        }
        private async Task CreateSockAsyncServer()
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(IPAddress.Any, 11000));
            _listener.Listen(10000);

            while (true)
            {
                try
                {
                    var client = await _listener.AcceptAsync();
                    int clientId = AddClient(client);

                    Debug.WriteLine($"Client connected: {client.RemoteEndPoint}");

                    _ = Task.Run(() => ReceiveLoopAsync(clientId, client));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Accept 실패: {ex.Message}");
                }
            }
        }

        private async Task ReceiveLoopAsync(int clientId, Socket client)
        {
            byte[] buffer = new byte[8192];
            try
            {
                while (true)
                {
                    int received = await client.ReceiveAsync(buffer, SocketFlags.None);
                    if (received == 0)
                    {
                        Debug.WriteLine("클라이언트 연결 종료됨.");
                        break;
                    }

                    string rcvData = Encoding.UTF8.GetString(buffer, 0, received);
                    Debug.WriteLine($"Received from {client.RemoteEndPoint}: {rcvData}");

                    var headerMatch = Regex.Match(rcvData, @"<HEADER>(.*?)</HEADER>");
                    var bodyMatch = Regex.Match(rcvData, @"<DATA>(.*?)</DATA>");

                    if (headerMatch.Success && bodyMatch.Success)
                    {
                        string command = headerMatch.Groups[1].Value;
                        string body = bodyMatch.Groups[1].Value;
                        //todo : 메시지 정합성 체크. xml 파일 비교.
                        //받은 xml deserialize 후 메시지 문자열 파싱하여 body를 dictionary 또는 리스트에 저장해서 service로 보내기 (데이터 정제)

                        //받는 부분이나, send도 마찬가지. xml로 serialzie 후 보낼 때 정합성 체크한 후 보내기 (정합성체크 와 보내는건 proxy 담당)
                        
                        var message = new SockMessage(clientId, command, body);
                        DataRecivedEventHandler?.Invoke(clientId, message);
                    }
                    else
                    {
                        Debug.WriteLine("HEADER 또는 BODY 태그 누락됨.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReceiveLoop 오류: {ex.Message}");
            }
            finally
            {
                client.Close();
                RemoveClient(clientId);
            }
        }

        private int AddClient(Socket socket)
        {
            int clientId = Interlocked.Increment(ref _nextClientId);
            _clients.TryAdd(clientId, socket);
            return clientId;
        }

        private void RemoveClient(int clientId)
        {
            _clients.TryRemove(clientId, out _);
        }

        public void SendMessage(SockMessage message)
        {
            if (_clients.TryGetValue(message.TableNo, out var client))
            {
                try
                {
                    var bytes = Encoding.UTF8.GetBytes(message.ToString()); // 예시. todo: 메시지 헤더 + body 넣을거임.
                    _ = client.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Send 실패: {ex.Message}");
                }
            }
        }

        public void Test()
        {
            DataRecivedEventHandler?.Invoke(1, new SockMessage(1, "Test"));
        }
    }
}
``
