using Microsoft.Extensions.DependencyInjection;
using RestaurantHost.Core.Enums;
using RestaurantHost.Core.Interfaces;
using RestaurantHost.Main.Interfaces;
using RestaurantHost.Main.Services;
using RestaurantHost.Main.ViewModels;
using RestaurantHost.Main.Views;
using RestaurantHost.Proxy.SockProxy;
using RestaurantHost.Support.Interfaces.XmlProtocol;
using RestaurantHost.Support.Services;

namespace RestaurantHost.Main
{
    public static class DIContainer
    {
        public static IServiceCollection AddMainLayer(this IServiceCollection services)
        {
            services.AddLogging();

            // Services
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<TableManagerService>();
            services.AddSingleton<TableStatusService>();

            // ViewModels
            services.AddSingleton<IMainViewModel, MainViewModel>();
            services.AddSingleton<TableStatusViewModel>();
            services.AddSingleton<PaymentHistoryViewModel>();

            // Views
            services.AddSingleton<MainPage>();
            services.AddSingleton<TableStatusView>();
            services.AddSingleton<PaymentHistoryView>();

            return services;
        }

        public static IServiceCollection AddSupportLayer(this IServiceCollection services)
        {
            // SockServerService는 ISocketMessageHandler로도 쓰이므로 여기서 단일 인스턴스로 등록
            var sockServerService = new SockServerService(SocketEnumType.Server); // 이거 이렇게 직접 new해서 조립하지말고 Support에서 조립하는게 좋다함.
            services.AddSingleton<ISocketMessageHandler>(sockServerService);
            services.AddSingleton<ISocketSenderMessageHandler>(sockServerService);
            services.AddSingleton<SockServerService>(provider =>
            {
                
                var handler = provider.GetRequiredService<ISocketSenderMessageHandler>();
                sockServerService.SetHandler(handler);
                return sockServerService;
            });
            // 기타 Support 서비스
            services.AddSingleton<ICommXmlProtocolService, CommXmlProtocolService>();

            return services;
        }

        public static IServiceCollection AddProxyLayer(this IServiceCollection services)
        {
            services.AddSingleton<SockServerProxy>(provider =>
            {
                var proxy = new SockServerProxy();
                var handler = provider.GetRequiredService<ISocketMessageHandler>();
                proxy.SetHandler(handler);
                return proxy;
            });

            return services;
        }
    }

}
