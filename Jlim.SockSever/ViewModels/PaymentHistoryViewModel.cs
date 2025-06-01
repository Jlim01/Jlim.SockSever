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
    class PaymentHistoryViewModel
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
            MessageBox.Show("enter 1");
        }
    }
}
