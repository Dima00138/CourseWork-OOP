using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseWork.Model;
using CourseWork.Services;
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
        private string _currentTable = "";
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

                //GetItems();

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

            switch (CurrentTable)
            {
                case "PASSENGERS":
                    Repository<Passenger> passengerRep = new PassengerRepository(Conn);
                    passengerRep.GetAll(RowMin, RowMax, Order, Items);
                    break;
                case "PAYMENTS":
                    Repository<Payment> paymentRep = new PaymentRepository(Conn);
                    paymentRep.GetAll(RowMin, RowMax, Order, Items);
                    break;
                case "ROUTES":
                    Repository<Route> routeRep = new RouteRepository(Conn);
                    routeRep.GetAll(RowMin, RowMax, Order, Items);
                    break;
                case "SCHEDULE":
                    Repository<Schedule> scheduleRep = new ScheduleRepository(Conn);
                    scheduleRep.GetAll(RowMin, RowMax, Order, Items);
                    break;
                case "STATIONS":
                    Repository<Station> stationRep = new StationRepository(Conn);
                    stationRep.GetAll(RowMin, RowMax, Order, Items);
                    break;
                case "STATIONS_ROUTES":
                    Repository<StationsRoute> stationsRouteRep = new StationsRouteRepository(Conn);
                    stationsRouteRep.GetAll(RowMin, RowMax, Order, Items);
                    break;
                case "TICKETS":
                    Repository<Ticket> ticketRep = new TicketRepository(Conn);
                    ticketRep.GetAll(RowMin, RowMax, Order, Items);
                    break;
                case "TRAINS":
                    Repository<Train> trainRep = new TrainRepository(Conn);
                    trainRep.GetAll(RowMin, RowMax, Order, Items);
                    break;
                case "VANS":
                    Repository<Van> vanRep = new VanRepository(Conn);
                    vanRep.GetAll(RowMin, RowMax, Order, Items);
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

        public void UpdateRows(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                switch (CurrentTable)
                {
                    case "PASSENGERS":
                        if (!Passenger.Update(sender, e, Conn))
                            e.Cancel = true;
                        break;
                    case "PAYMENTS":
                        if (!Payment.Update(sender, e, Conn))
                            e.Cancel = true;
                        break;
                    case "ROUTES":
                        if (!Route.Update(sender, e, Conn))
                            e.Cancel = true;
                        break;
                    case "SCHEDULE":
                        if (!Schedule.Update(sender, e, Conn))
                            e.Cancel = true;
                        break;
                    case "STATIONS":
                        if (!Station.Update(sender, e, Conn))
                            e.Cancel = true;
                        break;
                    case "STATIONS_ROUTES":
                        if (!StationsRoute.Update(sender, e, Conn))
                            e.Cancel = true;
                        break;
                    case "TICKETS":
                        if (!Ticket.Update(sender, e, Conn))
                            e.Cancel = true;
                        break;
                    case "TRAINS":
                        if (!Train.Update(sender, e, Conn))
                            e.Cancel = true;
                        break;
                    case "VANS":
                        if (!Van.Update(sender, e, Conn))
                            e.Cancel = true;
                        break;
                    default:
                        MessageBox.Show("Не существует такой таблицы", "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                        break;
                }
            }catch
            {
                e.Cancel = true;
            }
        }

        public void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) DeleteRows(sender, e);
            if (e.Key == Key.Enter) AddRow(sender); 
        }
        


        public void DeleteRows(object sender, KeyEventArgs e)
        {
            switch (CurrentTable)
            {
                case "PASSENGERS":
                    if (!Passenger.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        Items.Remove((sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "PAYMENTS":
                    if (!Payment.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        Items.Remove((sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "ROUTES":
                    if (!Route.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        Items.Remove((sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "SCHEDULE":
                    if (!Schedule.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        Items.Remove((sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "STATIONS":
                    if (!Station.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        Items.Remove((sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "STATIONS_ROUTES":
                    if (!StationsRoute.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        Items.Remove((sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "TICKETS":
                    if (!Ticket.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        Items.Remove((sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "TRAINS":
                    if (!Train.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        Items.Remove((sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "VANS":
                    if (!Van.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        Items.Remove((sender as DataGrid).SelectedCells[0].Item);
                    break;
                default:
                    MessageBox.Show("Не существует такой таблицы", "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    break;
            }
        }

        public void AddRow(object sender)
        {
            try
            {
                if ((sender as DataGrid).SelectedItem != null) return;

                switch (CurrentTable)
                {
                    case "PASSENGERS":
                        if (!Passenger.Insert(sender, Conn))
                            MessageBox.Show("Ошибка при добавлении строки");
                        break;
                    case "PAYMENTS":
                        if (!Payment.Insert(sender, Conn))
                            MessageBox.Show("Ошибка при добавлении строки");
                        break;
                    case "ROUTES":
                        if (!Route.Insert(sender, Conn))
                            MessageBox.Show("Ошибка при добавлении строки");
                        break;
                    case "SCHEDULE":
                        if (!Schedule.Insert(sender, Conn))
                            MessageBox.Show("Ошибка при добавлении строки");
                        break;
                    case "STATIONS":
                        if (!Station.Insert(sender, Conn))
                            MessageBox.Show("Ошибка при добавлении строки");
                        break;
                    case "STATIONS_ROUTES":
                        if (!StationsRoute.Insert(sender, Conn))
                            MessageBox.Show("Ошибка при добавлении строки");
                        break;
                    case "TICKETS":
                        if (!Ticket.Insert(sender, Conn))
                            MessageBox.Show("Ошибка при добавлении строки");
                        break;
                    case "TRAINS":
                        if (!Train.Insert(sender, Conn))
                            MessageBox.Show("Ошибка при добавлении строки");
                        break;
                    case "VANS":
                        if (!Van.Insert(sender, Conn))
                            MessageBox.Show("Ошибка при добавлении строки");
                        break;
                    default:
                        MessageBox.Show("Не существует такой таблицы", "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка в INSERT.fun");
            }
        }
    }
}
