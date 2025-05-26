using System.Windows;
using System.Windows.Controls;

namespace RestruantHost.Main.Services.MessengerService
{
    public class MenuChangeMsg
    {
        private string _viewModel;
        public MenuChangeMsg(string viewModel)
        {

            _viewModel = viewModel;

        }
    }
}
