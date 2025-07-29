using Microsoft.Extensions.DependencyInjection;
using RestaurantHost.Core.Enums;
using RestaurantHost.Core.Interfaces;
using RestaurantHost.Main.Views;
using RestaurantHost.Infrastructure.SockProxy;
using RestaurantHost.Service;
using RestaurantHost.Service.Interfaces.XmlProtocol;
using RestaurantHost.Service.Services;
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

            //서비스 제공자 생성
            _serviceProvider = services.BuildServiceProvider();

            //생성자 호출
            var proxy = _serviceProvider.GetRequiredService<SockServerProxy>();
            var handler = _serviceProvider.GetRequiredService<ISocketReceiveMessageHandler>();
            proxy.SetHandler(handler);
            _ = _serviceProvider.GetRequiredService<SockServerService>();
            var mainWindow = _serviceProvider.GetRequiredService<MainPage>();
            mainWindow.Show();
        }

        private static void RegistDIContainer(ServiceCollection services)
        {
            services.AddProxyLayer();   // SockServerProxy 생성 시 ISocketReceiveMessageHandler 필요
            services.AddSupportLayer(); // SockServerService + ISocketReceiveMessageHandler

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
