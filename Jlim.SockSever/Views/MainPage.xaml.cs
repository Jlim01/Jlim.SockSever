using MahApps.Metro.Controls;
using RestrauntHost.Main.Interfaces;

namespace RestrauntHost.Main.Views
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
