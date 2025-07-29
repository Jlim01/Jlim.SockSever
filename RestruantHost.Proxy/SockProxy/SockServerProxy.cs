using RestaurantHost.Core.Interfaces;
using RestaurantHost.Core.Models;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;

namespace RestaurantHost.Proxy.SockProxy
{
    public class SockServerProxy  : ISocketSenderMessageHandler
    {
        private Socket? _listener;
        private readonly ConcurrentDictionary<int, Socket> _clients = new();
        private int _nextClientId = 0;
        private int portNumber;
        private bool ListenRunning = true;
        private ISocketReceiveMessageHandler? _receiveHandler;

        public SockServerProxy()
        {
            portNumber = 11000; // 기본 포트 번호. 필요시 configure로 변경 가능.

            _ = CreateSockAsyncServer(); // fire-and-forget

            //포트번호 configure로 불러올수있음.
            //cancellation token 취소 시간 또한 configure 설정 가능
        }
        private async Task CreateSockAsyncServer()
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(IPAddress.Any, portNumber));
            _listener.Listen(10000);

            while (ListenRunning)
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
        public void SetHandler(ISocketReceiveMessageHandler handler)
        {
            _receiveHandler = handler;
            Debug.WriteLine($"[PROXY] Handler 세팅 완료: {_receiveHandler?.GetType().Name}");
        }

        // 호출되는 위치
        private void OnDataReceived(int clientId, SockMessage message)
        {
            Debug.WriteLine("[PROXY] OnDataReceived 호출됨");
            _receiveHandler?.OnMessageReceived(clientId, message);
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
                    Debug.WriteLine($"[PROXY] 수신된 HEADER: {headerMatch}, BODY: {bodyMatch}");
                    if (headerMatch.Success && bodyMatch.Success)
                    {
                        string header = headerMatch.Groups[1].Value;
                        string body = bodyMatch.Groups[1].Value;
                        //todo : 메시지 정합성 체크. xml 파일 비교.
                        //받은 xml deserialize 후 메시지 문자열 파싱하여 body를 dictionary 또는 리스트에 저장해서 service로 보내기 (데이터 정제)

                        //받는 부분이나, send도 마찬가지. xml로 serialzie 후 보낼 때 정합성 체크한 후 보내기 (정합성체크 와 보내는건 proxy 담당)
                        
                        var message = new SockMessage(clientId); // message를 파싱해야함.

                        OnDataReceived(clientId, message);
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
            if (_clients.TryGetValue(message.ModuleNo, out var client))
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

        public void OnMessageSend(int clientId, SockMessage message)
        {
            throw new NotImplementedException();
        }
    }
}