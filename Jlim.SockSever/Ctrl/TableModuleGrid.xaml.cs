using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestruantHost.Main.Ctrl
{
    /// <summary>
    /// Interaction logic for TableModuleGrid.xaml
    /// </summary>
    public partial class TableModuleGrid : UserControl
    {
        public TableModuleGrid()
        {
            InitializeComponent();
            RegisterEvent();
        }
        private void RegisterEvent()
        {
            TableGrid.MouseMove += TableModule_MouseMove;
            TableGrid.MouseLeave += TableModule_MouseLeave;
        }

        private void TableModule_MouseLeave(object sender, MouseEventArgs e)
        {
            quickTip.Visibility = Visibility.Collapsed;
            quickTipText.Visibility = Visibility.Collapsed;
        }
        private void TableModule_MouseMove(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;

            Point mousePos = e.GetPosition(grid);

            // 현재 셀 내부에서 퀵팁 위치 조정
            double offsetX = 10;
            double offsetY = 10;

            double tipWidth = quickTip.ActualWidth;
            double tipHeight = quickTip.ActualHeight;

            double maxLeft = grid.ActualWidth - tipWidth;
            double maxTop = grid.ActualHeight - tipHeight;

            double finalX = Math.Min(mousePos.X + offsetX, maxLeft);
            double finalY = Math.Min(mousePos.Y + offsetY, maxTop);

            quickTip.Margin = new Thickness(finalX, finalY, 0, 0);
            quickTip.Visibility = Visibility.Visible;
            quickTipText.Visibility = Visibility.Visible;
        }
       
    }
}
