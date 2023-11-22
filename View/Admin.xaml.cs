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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : UserControl
    {
        public Admin()
        {
            DataContext = new AdminViewModel();
            InitializeComponent();
        }

        private void Datagrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            var viewModel = DataContext as AdminViewModel;
            viewModel?.Items_Sorting(sender, e);
        }

        private void TableBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as AdminViewModel;
            viewModel?.Selection_Changed(sender, e);
        }
    }
}
