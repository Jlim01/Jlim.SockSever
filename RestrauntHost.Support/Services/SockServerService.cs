using RestaurantHost.Core.Enums;
using RestaurantHost.Core.Interfaces;
using RestaurantHost.Core.Models;
using System.Diagnostics;
namespace RestaurantHost.Service.Services
{
    public class SockServerService : ISocketSenderMessageHandler, ISocketReceiveMessageHandler
    {
        private readonly SocketEnumType _sockType;
        private ISocketSenderMessageHandler? _proxy;
        public SockServerService(ISocketSenderMessageHandler proxy)
        {
            _proxy = proxy;
        }


        public void OnMessageReceived(int clientId, SockMessage message)
        {
            try
            {
                OnMessageSend(clientId, message);

                if (Enum.TryParse<FromCliCmd>(message.Command, out var cmd))
                {
                    switch (cmd)
                    {
                        case FromCliCmd.S1F1:
                            // 처리
                            break;
                        case FromCliCmd.S2F1:
                            // 처리
                            break;
                        case FromCliCmd.S3F1C100:
                            break;
                        case FromCliCmd.S3F1C101:
                            break;
                        case FromCliCmd.S4F1:
                            break;
                        case FromCliCmd.S5F2:
                            break;
                        case FromCliCmd.S6F1:
                            break;
                        case FromCliCmd.S7F2:
                            break;
                        case FromCliCmd.S8F1:
                            break;
                        default:
                            // 처리 안 되는 경우
                            break;
                    }
                }
                else
                {
                    // enum에 해당하지 않는 "Accepted", "Disconnected" 같은 것 처리
                    switch (message.Command)
                    {
                        case "Accepted":
                            break;
                        case "Disconnected":
                            break;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        ///  OnMessageSend: service->proxy
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="message"></param>
        public void OnMessageSend(int clientId, SockMessage message)
        {
            _proxy?.OnMessageSend(clientId, message);

        }

    }
}
