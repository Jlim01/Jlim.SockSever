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
    public partial class TableStatusViewModel : ObservableObject
    {
        private int _rowCnt;
        private int _colCnt;
        public int RowCnt
        {
            get => _rowCnt;
            set => _rowCnt = value;
        }
        public int ColCnt
        {
            get => _colCnt;
            set => _colCnt = value;
        }
        private void RegisterMessage()
        {
            WeakReferenceMessenger.Default.Register<TableStatusViewModel, MenuChangeMsg>(this, (r, m) => OnChangeMenu(m));
        }

        private void OnChangeMenu(MenuChangeMsg m)
        {

        }
        [RelayCommand]
        private void RegisterCntBtn()
        {
            int T = 0;
        }
    }
}
