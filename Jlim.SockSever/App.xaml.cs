using Microsoft.Extensions.DependencyInjection;
using RestaurantHost.Core.Enums;
using RestaurantHost.Core.Interfaces;
using RestaurantHost.Main.Views;
using RestaurantHost.Proxy.SockProxy;
using RestaurantHost.Support;
using RestaurantHost.Support.Interfaces.XmlProtocol;
using RestaurantHost.Support.Services;
using System;
using System.Windows;

namespace RestaurantHost.Main
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            RegistDIContainer(services);

            // 5. 서비스 제공자 생성
            _serviceProvider = services.BuildServiceProvider();

            // 6. 진입점(MainPage) 생성 및 표시
            var mainWindow = _serviceProvider.GetRequiredService<MainPage>();
            _serviceProvider.GetRequiredService<SockServerProxy>();

            mainWindow.Show();
        }

        private static void RegistDIContainer(ServiceCollection services)
        {
            services.AddSupportLayer(); // SockServerService + ISocketMessageHandler
            services.AddProxyLayer();   // SockServerProxy 생성 시 ISocketMessageHandler 필요
            services.AddMainLayer();    // MainPage, ViewModel 등
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
