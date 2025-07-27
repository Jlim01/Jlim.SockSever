using RestaurantHost.Core.Enums;
using RestaurantHost.Proxy.SockProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    sockServerProxy.test();
          
                    
                    break;
                case SocketEnumType.Client:
                    break;
                default:
                    SockServerProxy = sockServerProxy;
                    break;

            }
        }
        
        private void OnClientReceivedEvent(int arg, SockMessage message)
        {
            try
            {
                switch (message.TransactionName)
                {
                    case "Accepted":
                        SendMessage(new SockMessage(1, "S1F1"); // sample 
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
        public void SendMessage(SockMessage message)
        {
            if (SockServerProxy != null)
            {
                SockServerProxy.SendMessage(message);
            }
            else
            {
                throw new InvalidOperationException("SockServerProxy is not initialized.");
            }
        }
    }
}
