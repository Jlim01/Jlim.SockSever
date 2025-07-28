using RestaurantHost.Core.Models;

namespace RestaurantHost.Core.Interfaces
{
    public interface ISocketMessageHandler
    {
        void OnMessageReceived(int clientId, SockMessage message);
    }
}
