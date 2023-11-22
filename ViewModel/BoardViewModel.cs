using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseWork.Model;
using MaterialDesignThemes.Wpf;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CourseWork.ViewModel
{
    public partial class BoardViewModel : ObservableObject
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

        [ObservableProperty]
        public ObservableCollection<UserSchedule> _items = new ObservableCollection<UserSchedule>();
        private OracleContext Conn { get; set; }


        public RelayCommand PrevButtonCommand { get; set; }
        public RelayCommand NextButtonCommand { get; set; }

        public BoardViewModel()
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

            string sql = $"SELECT * FROM MANAGER.TAKE_SCHEDULE_USER WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";

            using (OracleDataReader reader = Conn.SelectQuery(new OracleCommand(sql)))
            {
                while (reader.Read())
                {
                    if (reader.GetBoolean(11) == false) continue;

                    UserSchedule item = new UserSchedule();

                    item.Id = reader.GetInt64(0);
                    item.IdTrain = reader.GetInt64(1);
                    item.CategoryOfTrain = reader.GetString(2);
                    item.DeparturePoint = reader.GetString(3);
                    item.DepartureCity = reader.GetString(4);
                    item.ArrivalPoint = reader.GetString(5);
                    item.ArrivalCity = reader.GetString(6);
                    item.Distance = reader.GetInt64(7);
                    item.Duration = reader.GetInt64(8);
                    item.Date = reader.GetDateTime(9);
                    item.SetFrequency(reader.GetInt16(10));


                    Items.Add(item);
                }
            }
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
            }else
            {
                e.Column.SortDirection = ListSortDirection.Descending;
                sortDirection = "DESC";
            }

            GetItems($"{columnName}" + " " + sortDirection);
        }
    }
}