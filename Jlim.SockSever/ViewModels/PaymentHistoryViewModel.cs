using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RestaurantHost.Main.Services.MessengerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RestaurantHost.Main.ViewModels
{
    public partial class PaymentHistoryViewModel : ObservableObject
    {
        public PaymentHistoryViewModel()
        {
            RegisterMessage();       
        }
        private void RegisterMessage()
        {
            WeakReferenceMessenger.Default.Register<PaymentHistoryViewModel, MenuChangeMsg>(this, (r, m) => OnChangeMenu(m));
        }

        private void OnChangeMenu(MenuChangeMsg m)
        {

        }

        [RelayCommand]
        public void TestBtn()
        {
            MessageBox.Show("test");
        }
    }
}
