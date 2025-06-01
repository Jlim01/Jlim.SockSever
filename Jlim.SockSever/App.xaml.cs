using RestrauntHost.Main.Interfaces;
using RestrauntHost.Main.Services;
using RestrauntHost.Main.ViewModels;
using RestrauntHost.Main.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using RestruantHost.Main.Views;
using RestruantHost.Main.ViewModels;

namespace RestrauntHost.Main
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

            var mainWindow = _serviceProvider.GetRequiredService<MainPage>();
            _serviceProvider.GetRequiredService<PaymentHistoryView>();
            _serviceProvider.GetService<PaymentHistoryViewModel>();
            _serviceProvider.GetService<TableStatusView>();
            _serviceProvider.GetService<TableStatusViewModel>();

            mainWindow.Show();


        }

        private IServiceProvider ConfigureService(IServiceCollection services)
        {
            // Configure Logging
            //TODO: https://learn.microsoft.com/ko-kr/dotnet/core/extensions/logging?tabs=command-line  로그 파일 구분 환경 구축
            services.AddLogging();

            // Register Services
            services.AddSingleton<IUserService, UserService>();

            // Register ViewModels
            services.AddSingleton<IMainViewModel, MainViewModel>();
            services.AddSingleton<TableStatusViewModel>();
            services.AddSingleton<PaymentHistoryViewModel>();

            // Register Views
            services.AddSingleton<MainPage>();
            services.AddSingleton<TableStatusView>();
            services.AddSingleton<PaymentHistoryView>();


            return services.BuildServiceProvider();
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
