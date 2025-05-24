using CommunityToolkit.Mvvm.Input;
using RestrauntHost.Main.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RestrauntHost.Main.ViewModels
{
    public partial class MainViewModel :ObservableObject, IMainViewModel
    {
        private readonly ILogger<MainViewModel> _logger;
        private readonly IUserService _userService;
        public MainViewModel(ILogger<MainViewModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }


        [RelayCommand]
        private async Task SaveAsync()
        {
            try
            {
                _logger.LogInformation("Save command executed");
                await _userService.SaveUserDataAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during save operation");
            }
        }
    }
}
