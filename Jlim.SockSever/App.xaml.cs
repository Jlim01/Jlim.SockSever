using Microsoft.Extensions.DependencyInjection;
using RestaurantHost.Main.Views;
using System.Windows;
using RestaurantHost.Support;
using RestaurantHost.Support.Services;
namespace RestaurantHost.Main
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            // DI 등록
            services.AddSupportLayer(); // Support + Proxy 조립
            services.AddMainLayer();    // Main(Main ViewModel, View, Service) 조립

            // 서비스 제공자 구성
            _serviceProvider = services.BuildServiceProvider();

            // 진입점 뷰 생성 및 표시
            var mainWindow = _serviceProvider.GetRequiredService<MainPage>();
            _serviceProvider.GetRequiredService<SockServerService>();
            mainWindow.Show();
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
