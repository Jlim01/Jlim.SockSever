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
                switch (message.TransactionName)
                {
                    case "Accepted":
                        break;
                    case "Disconnected":
                        break;
                    case "S1F1":
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
