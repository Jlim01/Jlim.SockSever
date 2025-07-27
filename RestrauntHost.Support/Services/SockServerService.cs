using RestaurantHost.Core.Enums;
using RestaurantHost.Proxy.SockProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHost.Support.Services
{
    public class SockServerService
    {
        private SockServerProxy SockServerProxy;
        public SockServerService(SockServerProxy sockServerProxy, SocketEnumType sockType)
        {
            switch (sockType)
            {
                case SocketEnumType.Server:
                    SockServerProxy = sockServerProxy;
                    SockServerProxy.DataRecivedEventHandler += OnClientReceivedEvent;
                    break;
                case SocketEnumType.Client:
                    break;
                default:
                    SockServerProxy = sockServerProxy;
                    break;

            }
        }

        private void OnClientReceivedEvent(int arg1, SockMessage message)
        {
            try
            {
                if (Enum.TryParse<FromCliCmd>(message.TransactionName, out var cmd))
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
                    switch (message.TransactionName)
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
    }
}
