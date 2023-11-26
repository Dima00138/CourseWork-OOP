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
using System.Windows.Data;
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
        private string _currentTable = "PASSENGERS";
        [ObservableProperty]
        private ObservableCollection<DataGridColumn> columns;


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

            switch (CurrentTable)
            {
                case "PASSENGERS":
                    Repository<Passenger> passengerRep = new PassengerRepository(Conn);
                    Items = new ObservableCollection<object>(passengerRep.GetAll());
                    break;
                case "PAYMENTS":
                    Repository<Payment> paymentRep = new PaymentRepository(Conn);
                    Items = new ObservableCollection<object>(paymentRep.GetAll());
                    break;
                case "ROUTES":
                    Repository<Route> routeRep = new RouteRepository(Conn);
                    Items = new ObservableCollection<object>(routeRep.GetAll());
                    break;
                case "SCHEDULE":
                    Repository<Schedule> scheduleRep = new ScheduleRepository(Conn);
                    Items = new ObservableCollection<object>(scheduleRep.GetAll());
                    break;
                case "STATIONS":
                    Repository<Station> stationRep = new StationRepository(Conn);
                    Items = new ObservableCollection<object>(stationRep.GetAll());
                    break;
                case "STATIONS_ROUTES":
                    Repository<StationsRoute> stationsRouteRep = new StationsRouteRepository(Conn);
                    Items = new ObservableCollection<object>(stationsRouteRep.GetAll());
                    break;
                case "TICKETS":
                    Repository<Ticket> ticketRep = new TicketRepository(Conn);
                    Items = new ObservableCollection<object>(ticketRep.GetAll());
                    break;
                case "TRAINS":
                    Repository<Train> trainRep = new TrainRepository(Conn);
                    Items = new ObservableCollection<object>(trainRep.GetAll());
                    break;
                case "VANS":
                    Repository<Van> vanRep = new VanRepository(Conn);
                    Items = new ObservableCollection<object>(vanRep.GetAll());
                    break;
                default:
                    MessageBox.Show("Не существует такой таблицы", "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    break;
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

        public void UpdateRows(object sender, DataGridRowEditEndingEventArgs e)
        {
            Passenger editedItem = e.Row.DataContext as Passenger;
            Passenger item = e.Row.Item as Passenger;

            if (editedItem != null)
            {
                if (string.IsNullOrEmpty(editedItem.FullName))
                {
                    MessageBox.Show("Пожалуйста, введите полное имя пассажира.");
                    e.Cancel = true;
                }
                Repository<Passenger> passengerRep = new PassengerRepository(Conn);
                passengerRep.Update(editedItem);
            }
        }

        public void CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString()?.ToLower() == "id")
            {
                MessageBox.Show("Редактировать Id нельзя");
                e.Cancel = true;
                return;
            }
            var item = e.Row.DataContext as Passenger;
            string? col = e.Column.Header.ToString();
            string newVal = (e.EditingElement as TextBox).Text;
            
        }

        public void DeleteRows(object sender, KeyEventArgs e)
        {
            
        }

        public void AddItem(object sender, AddingNewItemEventArgs e) 
        {

        }
    }
}
