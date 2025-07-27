using Microsoft.Extensions.DependencyInjection;
using RestaurantHost.Main.Interfaces;
using RestaurantHost.Main.Services;
using RestaurantHost.Main.ViewModels;
using RestaurantHost.Main.Views;

namespace RestaurantHost.Main
{
    public static class MainContainer
    {
        public static IServiceCollection AddMainLayer(this IServiceCollection services)
        {

            // Configure Logging
            //TODO: https://learn.microsoft.com/ko-kr/dotnet/core/extensions/logging?tabs=command-line  로그 파일 구분 환경 구축
            services.AddLogging();

            //Register Services
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

            return services;
        }
    }
}
