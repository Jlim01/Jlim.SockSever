using RestaurantHost.Core.Enums;
using RestaurantHost.Core.Interfaces;
using RestaurantHost.Core.Models;
using System.Diagnostics;
namespace RestaurantHost.Support.Services
{
    public class SockServerService : ISocketSenderMessageHandler, ISocketReceiveMessageHandler
    {
        private readonly SocketEnumType _sockType;
        private ISocketSenderMessageHandler? _senderHandler;
        public SockServerService(SocketEnumType sockType)
        {
            _sockType = sockType;
        }


        public void OnMessageReceived(int clientId, SockMessage message)
        {
            try
            {
                OnMessageSend(clientId, message);
                if (_sockType == SocketEnumType.Client)
                {

                }
                else if (_sockType == SocketEnumType.Server)
                {
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
            }
            catch (Exception ex)
            {

            }
        }

        public void OnMessageSend(int clientId, SockMessage message)
        {
            _senderHandler?.OnMessageSend(clientId, message);

        }
        public void SetHandler(ISocketSenderMessageHandler handler)
        {
            _senderHandler = handler;
            Debug.WriteLine($"[SUPPORT.Service] Handler 세팅 완료: {_senderHandler?.GetType().Name}");
        }
    }
}
