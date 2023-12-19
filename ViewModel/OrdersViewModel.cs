using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseWork.Model;

namespace CourseWork.ViewModel
{
    public partial class OrdersViewModel : ObservableObject
    {
        private int _rowMin = 0;
        private int _rowMax = 50;
        private string Order = "ID DESC";

        public int RowMin
        {
            get { return _rowMin; }
            set { _rowMin = value; }
        }

        public int RowMax
        {
            get => _rowMax;
            set { _rowMax = value; }
        }

        public string SearchPassenger { get; set; } = "";
        [ObservableProperty]
        public ObservableCollection<TakeTicket> _items = new();

        private OracleContext Conn { get; set; }


        public RelayCommand PrevButtonCommand { get; set; }
        public RelayCommand NextButtonCommand { get; set; }
        public RelayCommand SearchButtonCommand { get; set; }

        public OrdersViewModel()
        {
            try
            {
                Conn = OracleContext.Create();

                GetItems();

                PrevButtonCommand = new RelayCommand(() =>
                {
                    try
                    {
                    if (RowMin <= 0) return;
                    RowMin -= 50;
                    RowMax -= 50;
                    GetItems();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка перехода на другую страницу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                });
                NextButtonCommand = new RelayCommand(() =>
                {
                    try
                    {
                    RowMin += 50;
                    RowMax += 50;
                    GetItems();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка перехода на другую страницу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
                SearchButtonCommand = new RelayCommand(() =>
                {
                    try
                    {
                        if (SearchPassenger == "") GetItems();
                        string IdPas = Convert.ToInt64(SearchPassenger).ToString();
                        GetItems(IdPas);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка");
                    }
                });
            }
            catch
            { }
        }

        private void GetItems(string _SearchPassenger = "")
        {
            try
            {
            OrdersRepository orderRep = new OrdersRepository(Conn);
            Items.Clear();
            orderRep.GetAll(RowMin, RowMax, _SearchPassenger, Order, Items);
            }
            catch
            {
                MessageBox.Show("Ошибка получения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void Items_Sorting(object sender, DataGridSortingEventArgs e)
        {
            try
            {
            e.Handled = true;

            string columnNameUnusable = e.Column.SortMemberPath;
            string sortDirection;

            string[] substrings = Regex.Split(columnNameUnusable, @"(?<!^)(?=[A-Z])");

            string columnName = string.Join("_", substrings);

            if (columnName.ToUpper() == "DATE") columnName = "\"DATE\"";
            if (columnName.ToUpper() == "ID") columnName = "\"ID\"";

            if (Order.Split(' ')[1].ToUpper() == "DESC")
            {
                e.Column.SortDirection = ListSortDirection.Ascending;
                sortDirection = "ASC";
            }
            else
            {
                e.Column.SortDirection = ListSortDirection.Descending;
                sortDirection = "DESC";
            }

            Order = $"{columnName}" + " " + sortDirection;

            GetItems();
            }
            catch
            {
                MessageBox.Show("Ошибка сортировки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateRows(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (!TakeTicket.Update(sender, e, Conn, Items))
                    e.Cancel = true;

            }
            catch
            {
                e.Cancel = true;
                MessageBox.Show("Ошибка обновления данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) DeleteRows(sender, e);
            if (e.Key == Key.Enter) AddRow(sender);
        }



        public void DeleteRows(object sender, KeyEventArgs e)
        {
            try
            {
                MessageBoxResult res = MessageBox.Show("Вы действительно хотите удалить строку?", "Подтвердите удаление",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res != MessageBoxResult.Yes) return;

                if (!TakeTicket.Delete(sender, e, Conn))
                    MessageBox.Show("Ошибка при удалении строки");
                else
                    Items.Remove((TakeTicket)(sender as DataGrid).SelectedCells[0].Item);
            }
            catch
            {
                MessageBox.Show("Ошибка удаления данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddRow(object sender)
        {
            try
            {
                if ((sender as DataGrid).SelectedItem != null) return;


                if (!TakeTicket.Insert(sender, Conn))
                    MessageBox.Show("Ошибка при добавлении строки");
            }
            catch
            {
                MessageBox.Show("Ошибка добавления строки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
