using CommunityToolkit.Mvvm.Input;
using RestrauntHost.Main.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestrauntHost.Main.ViewModels
{
    public class MainViewModel : IMainViewModel
    {
        private readonly ILogger<MainViewModel> _logger;
        private readonly IUserService _userService;
        public MainViewModel(ILogger<MainViewModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        private ICommand _saveCommand;
        public ICommand SaveCommand =>
            _saveCommand ??= new AsyncRelayCommand(ExecuteSaveAsync);

        private async Task ExecuteSaveAsync()
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
