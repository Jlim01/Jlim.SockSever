using CommunityToolkit.Mvvm.Messaging;
using RestruantHost.Main.Services.MessengerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RestruantHost.Main.ViewModels
{
    class TableStatusViewModel
    {
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
