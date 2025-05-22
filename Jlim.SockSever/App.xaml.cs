using Jlim.SockSever.Interfaces;
using Jlim.SockSever.Services;
using Jlim.SockSever.ViewModels;
using Jlim.SockSever.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Jlim.SockSever
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// ConfigureService에 의존성 주입을 위한 서비스/뷰/뷰모델 등록
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureService(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureService(IServiceCollection services)
        {
            // Configure Logging
            //TODO: https://learn.microsoft.com/ko-kr/dotnet/core/extensions/logging?tabs=command-line  로그 파일 구분 환경 구축
            services.AddLogging();

            // Register Services
            services.AddSingleton<IUserService, UserService>();

            // Register ViewModels
            services.AddSingleton<IMainViewModel, MainViewModel>();

            // Register Views
            services.AddSingleton<MainWindow>();

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
