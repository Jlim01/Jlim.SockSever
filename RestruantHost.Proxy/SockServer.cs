using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;

namespace RestaurantHost.Proxy
{
    public class SockServer
    {

        static async Task  CreateSockAsyncServer()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, 11000));
            listener.Listen(10000);


            Debug.WriteLine("Waiting for client...");
            Socket client = await listener.AcceptAsync();
            Debug.WriteLine("Client connected.");

            var receiveTask = ReceiveLoopAsync(client);
            var sendTask = SendLoopAsync(client);

            await Task.WhenAny(receiveTask, sendTask);

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
                }
                catch
                {
                    Debug.WriteLine("Disconnected during receive.");
                    break;
                }
                string rcvData = Encoding.UTF8.GetString(buffer, 0, received);


                Debug.WriteLine(rcvData);
            }
        }

        static async Task SendLoopAsync(Socket client)
        {
            while (true)
            {

                Debug.Write("SEND MSG_ID 입력 > ");
                string input = Console.ReadLine();

                //Command 찾기
                if (!commandDict.TryGetValue(input, out string xml))
                {
                    Console.WriteLine("존재하지 않는 명령입니다.");
                    continue;
                }

                //send
                xml = xml.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");

                #region header 장비 id
                string equipId = "PNE_PPC_1F_0";
                var match = Regex.Match(xml, @"<EQP_ID>\s*</EQP_ID>");
                if (match.Success)
                {
                    Console.Write("equip 번호: ");
                    equipId += Console.ReadLine();

                    xml = Regex.Replace(xml, @"<EQP_ID>\s*</EQP_ID>", $"<EQP_ID>{equipId}</EQP_ID>");
                }
                #endregion

                #region body  -> 하드코딩으로 대체. 입력할게 많으면 비효율. 헤더만 입력으로.
                xml = InputXmlBodyContents(xml);
                #endregion
                byte[] sendData = XmlFormat.SendDataFormat(xml);

                try
                {
                    await client.SendAsync(sendData, SocketFlags.None);
                }
                catch
                {
                    Console.WriteLine("Disconnected during send.");
                    break;
                }

                //output
                string sendStr = Encoding.UTF8.GetString(sendData);
                var sendMatch = Regex.Match(sendStr, @"<MSG_ID>(.*?)</MSG_ID>");
                if (sendMatch.Success)
                {
                    if (!string.IsNullOrEmpty(equipId))
                    {
                        Console.Write($"[{equipId}]\t");
                    }
                    Console.WriteLine($"Server(IMS) -> Client(CTSMON): {sendMatch.Groups[1].Value}");
                }

                else
                    Console.WriteLine("Server(IMS) -> Client(CTSMON): <MSG_ID> 태그를 찾을 수 없습니다.");

                Console.WriteLine(sendStr);
            }
        }

    }
}
