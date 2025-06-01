using CommunityToolkit.Mvvm.Input;
using RestaurantHost.Main.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using RestaurantHost.Main.Services.MessengerService;

namespace RestaurantHost.Main.ViewModels
{
    public partial class MainViewModel :ObservableObject, IMainViewModel
    {
        private readonly ILogger<MainViewModel> _logger;
        private readonly IUserService _userService;

        public Dictionary<string, bool> MenuChecked { get; set; } = new();
        public MainViewModel(ILogger<MainViewModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;

            InitMenuChecked();
            RegisterMessage();
        }

        private void RegisterMessage()
        {

        }

        private void InitMenuChecked()
        {
            if(MenuChecked.Count > 0) MenuChecked.Clear();
            MenuChecked.Add("TableStatusViewModel", true);
            MenuChecked.Add("PaymentHistoryViewModel", false);
        }

        [RelayCommand]
        private void ChangeMenuBtn()
        {
           foreach(KeyValuePair<string, bool> pair in MenuChecked)
            {
                if(pair.Value == true)
                {
                    MessageBox.Show(pair.Key);
                    WeakReferenceMessenger.Default.Send(new MenuChangeMsg(pair.Key.ToString()));
                }
            }

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
