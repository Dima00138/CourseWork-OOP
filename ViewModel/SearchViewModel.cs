﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseWork.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CourseWork.ViewModel
{
    public partial class SearchViewModel : ObservableObject
    {
        private int _rowMin = 0;
        private int _rowMax = 50;
        private string Where;
        private string Order;

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
        private OracleContext conn;

        private ScheduleRepository rep;


        public RelayCommand PrevButtonCommand { get; set; }
        public RelayCommand NextButtonCommand { get; set; }


        public SearchViewModel(string _Where = "ID > 0")
        {
            try
            {
                Where = _Where;
                Order = "ID desc";

                conn = OracleContext.Create();
                rep = new ScheduleRepository(conn);
                GetItems(Where, Order);

                PrevButtonCommand = new RelayCommand(() =>
                {
                    try
                    {
                        if (RowMin <= 0) return;
                        RowMin -= 50;
                        RowMax -= 50;
                        GetItems(Where, Order);
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
                        GetItems(Where, Order);
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка перехода на другую страницу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
            catch
            { }
        }

        private void GetItems(string Where, string Order = "ID desc")
        {
            try
            {
                Items.Clear();
                rep.TakeSchedule_User(RowMin, RowMax, Where, Order, Items);
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
                Order = $"{columnName}" + " " + sortDirection;
                GetItems(Where, Order);
            }
            catch
            {
                MessageBox.Show("Ошибка сортировки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}