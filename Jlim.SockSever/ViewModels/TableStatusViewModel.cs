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
    class TableStatusViewModel
    {
        public TableStatusViewModel()
        {
            int t = 0;
        }
        private void RegisterMessage()
        {
            WeakReferenceMessenger.Default.Register<TableStatusViewModel, MenuChangeMsg>(this, (r, m) => OnChangeMenu(m));
        }

        private void OnChangeMenu(MenuChangeMsg m)
        {
            MessageBox.Show("enter 2");
        }
    }
}
