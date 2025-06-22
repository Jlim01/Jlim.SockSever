using RestaurantHost.Main.Interfaces;
using RestaurantHost.Main.Services;
using RestaurantHost.Main.ViewModels;
using RestaurantHost.Main.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using RestaurantHost.Main.Views;
using RestaurantHost.Main.ViewModels;
using System;
using RestaurantHost.Support.Interfaces.XmlProtocol;
using RestaurantHost.Support.Services;
using RestaurantHost.Proxy.SockProxy;
using RestaurantHost.Core.Enums;

namespace RestaurantHost.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// ConfigureService에 의존성 주입을 위한 서비스/뷰/뷰모델 등록
    /// </summary>
    /// 
    /*
     * https://mahapps.com/docs/controls/HamburgerMenu  메뉴 네비게이션 만들때 이거 사용하기.
     */
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureService(serviceCollection);
            _serviceProvider  = serviceCollection.BuildServiceProvider();
            var mainWindow = _serviceProvider.GetRequiredService<MainPage>();
            _serviceProvider.GetRequiredService<PaymentHistoryView>();
            _serviceProvider.GetRequiredService<PaymentHistoryViewModel>();
            _serviceProvider.GetRequiredService<TableStatusView>();
            _serviceProvider.GetRequiredService<TableStatusViewModel>();

            //Services
            _serviceProvider.GetRequiredService<TableManagerService>();
            _serviceProvider.GetRequiredService<SockServerService>();
            mainWindow.Show();


        }

        private void ConfigureService(IServiceCollection services)
        {
            // Configure Logging
            //TODO: https://learn.microsoft.com/ko-kr/dotnet/core/extensions/logging?tabs=command-line  로그 파일 구분 환경 구축
            services.AddLogging();

            // Register Services
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<TableManagerService>();
            services.AddSingleton<TableStatusService>();

            // Register ViewModels
            services.AddSingleton<IMainViewModel, MainViewModel>();
            services.AddSingleton<TableStatusViewModel>();
            services.AddSingleton<PaymentHistoryViewModel>();

            // Register Views
            services.AddSingleton<MainPage>();
            services.AddSingleton<TableStatusView>();
            services.AddSingleton<PaymentHistoryView>();

            //Proxy
            services.AddSingleton<SockServerProxy>();

            //Support
            services.AddSingleton<ICommXmlProtocolService, CommXmlProtocolService>();
            services.AddSingleton<SockServerService>(provider =>
            {
                var proxy = provider.GetRequiredService<SockServerProxy>();
                return new SockServerService(proxy, SocketEnumType.Server);
            });  //enum은 di 주입이 안됨에 따른 Factory 패턴을 통해 GetRequiredService() 발생 시 내부로 들어와 enum값 넘김. 
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            // Dispose of services if needed
            if(_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

}
