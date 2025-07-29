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
using System.Xaml;
using RestaurantHost.Service.Interfaces.XmlProtocol;

namespace RestaurantHost.Main.ViewModels
{
    public partial class MainViewModel :ObservableObject, IMainViewModel
    {
        private readonly ILogger<MainViewModel> Logger;
        private readonly IUserService UserService;
        private ICommXmlProtocolService XmlService;
        #region command
        public ICommand ChangeMenuBtnCommand { get; set; }

        #endregion
        public TableStatusViewModel TableStatusViewModel { get; }
        public PaymentHistoryViewModel PaymentHistoryViewModel { get; }

        public Dictionary<string, bool> MenuChecked { get; set; } = new();
        public MainViewModel(TableStatusViewModel tableStatusVM, 
                             PaymentHistoryViewModel paymentHistoryVM, 
                             ILogger<MainViewModel> logger,
                             IUserService userService,
                             ICommXmlProtocolService xmlService)
        {
            TableStatusViewModel = tableStatusVM;
            PaymentHistoryViewModel = paymentHistoryVM;
            Logger = logger;
            UserService = userService;
            XmlService = xmlService;

            InitMenuChecked();
            RegisterMessage();
        }

        private void RegisterMessage()
        {
            ChangeMenuBtnCommand = new RelayCommand(ChangeMenuBtn);
        }

        private void InitMenuChecked()
        {
            if(MenuChecked.Count > 0) MenuChecked.Clear();
            MenuChecked.Add("TableStatusViewModel", true);
            MenuChecked.Add("PaymentHistoryViewModel", false);
        }


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
                Logger.LogInformation("Save command executed");
                await UserService.SaveUserDataAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error during save operation");
            }
        }
    }
}
