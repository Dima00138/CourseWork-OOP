using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseWork.Model;
using MaterialDesignThemes.Wpf;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace CourseWork.ViewModel
{
    public partial class AdminViewModel : ObservableObject
    {
        private int _rowMin = 0;
        private int _rowMax = 50;

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


        public string[] Tables { get; set; } = { "PASSENGERS", "PAYMENTS", "ROUTES",
                                    "SCHEDULE", "STATIONS", "STATIONS_ROUTES",
                                    "TICKETS", "TRAINS", "VANS"};


        [ObservableProperty]
        public ObservableCollection<object> _items = new ObservableCollection<object>();

        [ObservableProperty]
        public string _currentTable = "";


        private OracleContext Conn { get; set; }


        public RelayCommand PrevButtonCommand { get; set; }
        public RelayCommand NextButtonCommand { get; set; }

        public AdminViewModel()
        {
            try
            {

                Conn = OracleContext.Create();

                GetItems();

                PrevButtonCommand = new RelayCommand(() =>
                {
                    if (RowMin <= 0) return;
                    RowMin -= 50;
                    RowMax -= 50;
                    GetItems();

                });
                NextButtonCommand = new RelayCommand(() =>
                {
                    RowMin += 50;
                    RowMax += 50;
                    GetItems();
                });
            }
            catch
            { }
        }

        private void GetItems(string Order = "ID desc")
        {
            Items.Clear();
            Items = new ObservableCollection<object>();

            string sql = $"SELECT * FROM MANAGER.{CurrentTable} WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
            

            using (OracleDataReader reader = Conn.SelectQuery(new OracleCommand(sql)))
            {
                while (reader.Read())
                {
                    switch (CurrentTable)
                    {
                        case "PASSENGERS":
                            //PassengerRepository rep = new PassengerRepository(Conn);
                            break;
                        case "PAYMENTS":
                             
                            break;
                        case "ROUTES":
                             
                            break;
                        case "SCHEDULE":


                             break;
                        case "STATIONS":
                            

                            break;
                        case "STATIONS_ROUTES":
                            

                            break;
                        case "TICKETS":
                            

                            break;
                        case "TRAINS":
                            
                            
                            break;
                        case "VANS":
                           

                            break;
                        default:
                            MessageBox.Show("Не существует такой таблицы", "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                            break;
                    }
                }
            }
        }

        public void Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            CurrentTable = Tables[box.SelectedIndex];
            GetItems();
        }

        public void Items_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;

            string columnNameUnusable = e.Column.SortMemberPath;
            string sortDirection;

            string[] substrings = Regex.Split(columnNameUnusable, @"(?<!^)(?=[A-Z])");

            string columnName = string.Join("_", substrings);

            if (columnName.ToUpper() == "DATE") columnName = "\"DATE\"";
            if (columnName.ToUpper() == "ID") columnName = "\"ID\"";

            if (e.Column.SortDirection == ListSortDirection.Descending)
            {
                e.Column.SortDirection = ListSortDirection.Ascending;
                sortDirection = "ASC";
            }
            else
            {
                e.Column.SortDirection = ListSortDirection.Descending;
                sortDirection = "DESC";
            }

            GetItems($"{columnName}" + " " + sortDirection);
        }

        public void RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            
        }
        public void DeleteRows(object sender, KeyEventArgs e)
        {
            
        }

        public void UpdateRows(object sender, DataGridCellEditEndingEventArgs e)
        {

        }
    }
}
