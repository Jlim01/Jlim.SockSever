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
            services.AddSingleton<ISocketReceiveMessageHandler, SockServerService>();
            services.AddSingleton<SockServerService>(); // 필요 시 명시적 등록
            // 기타 Support 서비스
            services.AddSingleton<ICommXmlProtocolService, CommXmlProtocolService>();

            return services;
        }

        public static IServiceCollection AddProxyLayer(this IServiceCollection services)
        {
            services.AddSingleton<SockServerProxy>();
            services.AddSingleton<ISocketSenderMessageHandler>(provider =>
            {
                // proxy 인스턴스를 여기서도 등록
                return provider.GetRequiredService<SockServerProxy>();
            });
            return services;
        }
    }

}
