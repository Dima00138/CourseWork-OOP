﻿using CourseWork.ViewModel;
using CourseWork.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : UserControl
    {
        public Orders()
        {
            InitializeComponent();
        }

        private void Datagrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            var viewModel = DataContext as OrdersViewModel;
            viewModel?.Items_Sorting(sender, e);
        }

        private void Datagrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as OrdersViewModel;
            viewModel?.PreviewKeyDown(sender, e);
        }

        private void Datagrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var viewModel = DataContext as OrdersViewModel;
            viewModel?.UpdateRows(sender, e);
        }

        private void Datagridd_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime)) // Если тип данных столбца - DateTime
            {
                DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
                dataGridTextColumn.Header = e.Column.Header;
                dataGridTextColumn.Binding = new Binding(e.PropertyName)
                {
                    Converter = new DateTimeToStringConverter(), // Ваш конвертер даты
                    StringFormat = "dd/MM/yyyy hh:mm tt" // Формат даты
                };
                e.Column = dataGridTextColumn;
            }
        }
    }
}
