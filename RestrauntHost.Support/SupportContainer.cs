using RestaurantHost.Support.Interfaces.XmlProtocol;
using RestaurantHost.Support.Services;
using Microsoft.Extensions.DependencyInjection;
using RestaurantHost.Core.Enums;
using RestaurantHost.Proxy.SockProxy;


namespace RestaurantHost.Support
{
    public static class SupportContainer
    {
        public static IServiceCollection AddSupportLayer(this IServiceCollection services)
        {
            services.AddSingleton<SockServerProxy>();

            //Support
            services.AddSingleton<ICommXmlProtocolService, CommXmlProtocolService>();
            services.AddSingleton<SockServerService>(provider =>
            {
                var proxy = provider.GetRequiredService<SockServerProxy>();
                return new SockServerService(proxy, SocketEnumType.Server);
            });  //enum은 di 주입이 안됨에 따른 Factory 패턴을 통해 GetRequiredService() 발생 시 내부로 들어와 enum값 넘김. 
          
            return services;
        }
    }
}

