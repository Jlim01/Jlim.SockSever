using RestruantHost.Main.Ctrl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using static System.Net.Mime.MediaTypeNames;

namespace RestaurantHost.Main.Views
{
    /// <summary>
    /// Interaction logic for TableStatusView.xaml
    /// </summary>
    /// 
    public partial class TableStatusView : UserControl
    {
        int maxRow = 1;
        int maxCol = 1; // default is 1
        int newMaxRow = 1;
        int newMaxCol = 1;
        public TableStatusView()
        {
            InitializeComponent();

            InitRowColTable(maxRow, maxCol);

        }

        private void InitRowColTable(int maxRow, int maxCol)
        {
            InitModuleGrid();

            //maxRow , maxCol 설정
            for (int rowIdx = 0; rowIdx < maxRow; ++rowIdx)
            {
                modulesGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int colIdx = 0; colIdx < maxCol; ++colIdx)
            {
                modulesGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int y = 0; y < maxRow; ++y)
            {

                for (int x = 0; x < maxCol; ++x)
                {
                    var test = new TableModuleGrid();
                    test.quickTipText.Text = $"{y + 1}-{x + 1}";
                    test.quickTip.Visibility = Visibility.Collapsed;

                    Grid.SetColumn(test, x);
                    Grid.SetRow(test, y);

                    modulesGrid.Children.Add(test);

                }
            }
        }

        private void InitModuleGrid()
        {
            modulesGrid.Children.Clear();
            modulesGrid.RowDefinitions.Clear();
            modulesGrid.ColumnDefinitions.Clear();
        }

        private void TextRowChanged(object sender, TextChangedEventArgs e)
        {

            if (sender is TextBox tb)
            {
                // 사용자가 직접 편집한 경우에만 반응
                if (!tb.IsKeyboardFocusWithin) return;
            }
                if (sender is TextBox textBox && int.TryParse(textBox.Text, out int newRow))
            {
                if(maxRow != newRow)
                {
                    maxRow = newRow;
                    InitRowColTable(maxRow, maxCol);
                }
            }
        }

        private void TextColChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                // 사용자가 직접 편집한 경우에만 반응
                if (!tb.IsKeyboardFocusWithin) return;
            }
            if (sender is TextBox textBox && int.TryParse(textBox.Text, out int newCol))
            {
                if (maxCol != newCol)
                {
                    maxCol = newCol;
                    InitRowColTable(maxRow, maxCol);
                }
            }
        }
    }
}
