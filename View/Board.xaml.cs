using CourseWork.ViewModel;
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

namespace CourseWork.View
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
        public Board()
        {
            DataContext = new BoardViewModel();
            InitializeComponent();
        }

        private void Datagrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            var viewModel = DataContext as BoardViewModel;
            viewModel?.Items_Sorting(sender, e);
        }
    }
}
