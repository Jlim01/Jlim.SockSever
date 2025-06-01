using MahApps.Metro.Controls;
using RestaurantHost.Main.Interfaces;

namespace RestaurantHost.Main.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : MetroWindow
    {
        public MainPage(IMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
