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

        public string[] Tables { get; set; } = { "PASSENGERS", "PAYMENTS", "ROUTES",
                                    "SCHEDULE", "STATIONS", "STATIONS_ROUTES",
                                    "TICKETS", "TRAINS", "VANS"};

        [ObservableProperty]
        public ObservableCollection<Passenger> _itemsPassenger = new();
        [ObservableProperty]
        public ObservableCollection<Payment> _itemsPayment = new();
        [ObservableProperty]
        public ObservableCollection<Route> _itemsRoute = new();
        [ObservableProperty]
        public ObservableCollection<Schedule> _itemsSchedule = new();
        [ObservableProperty]
        public ObservableCollection<Station> _itemsStation = new();
        [ObservableProperty]
        public ObservableCollection<StationsRoute> _itemsStationsRoute = new();
        [ObservableProperty]
        public ObservableCollection<Ticket> _itemsTicket = new();
        [ObservableProperty]
        public ObservableCollection<Train> _itemsTrain = new();
        [ObservableProperty]
        public ObservableCollection<Van> _itemsVan = new();

        [ObservableProperty]
        private string _currentTable = "";

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

        private void GetItems(DataGrid dg = null)
        {
            switch (CurrentTable)
            {
                case "PASSENGERS":
                    Repository<Passenger> passengerRep = new PassengerRepository(Conn);
                    if (dg != null) dg.ItemsSource = ItemsPassenger;
                    ItemsPassenger.Clear();
                    passengerRep.GetAll(RowMin, RowMax, Order, ItemsPassenger);
                    break;
                case "PAYMENTS":
                    Repository<Payment> paymentRep = new PaymentRepository(Conn);
                    if (dg != null) dg.ItemsSource = ItemsPayment;
                    ItemsPayment.Clear();
                    paymentRep.GetAll(RowMin, RowMax, Order, ItemsPayment);
                    break;
                case "ROUTES":
                    Repository<Route> routeRep = new RouteRepository(Conn);
                    if (dg != null) dg.ItemsSource = ItemsRoute;
                    ItemsRoute.Clear();
                    routeRep.GetAll(RowMin, RowMax, Order, ItemsRoute);
                    break;
                case "SCHEDULE":
                    Repository<Schedule> scheduleRep = new ScheduleRepository(Conn);
                    if (dg != null) dg.ItemsSource = ItemsSchedule;
                    ItemsSchedule.Clear();
                    scheduleRep.GetAll(RowMin, RowMax, Order, ItemsSchedule);
                    break;
                case "STATIONS":
                    Repository<Station> stationRep = new StationRepository(Conn);
                    if (dg != null) dg.ItemsSource = ItemsStation;
                    ItemsStation.Clear();
                    stationRep.GetAll(RowMin, RowMax, Order, ItemsStation);
                    break;
                case "STATIONS_ROUTES":
                    Repository<StationsRoute> stationsRouteRep = new StationsRouteRepository(Conn);
                    if (dg != null) dg.ItemsSource = ItemsStationsRoute;
                    ItemsStationsRoute.Clear();
                    stationsRouteRep.GetAll(RowMin, RowMax, Order, ItemsStationsRoute);
                    break;
                case "TICKETS":
                    Repository<Ticket> ticketRep = new TicketRepository(Conn);
                    if (dg != null) dg.ItemsSource = ItemsTicket;
                    ItemsTicket.Clear();
                    ticketRep.GetAll(RowMin, RowMax, Order, ItemsTicket);
                    break;
                case "TRAINS":
                    Repository<Train> trainRep = new TrainRepository(Conn);
                    if (dg != null) dg.ItemsSource = ItemsTrain;
                    ItemsTrain.Clear();
                    trainRep.GetAll(RowMin, RowMax, Order, ItemsTrain);
                    break;
                case "VANS":
                    Repository<Van> vanRep = new VanRepository(Conn);
                    if (dg != null) dg.ItemsSource = ItemsVan;
                    ItemsVan.Clear();
                    vanRep.GetAll(RowMin, RowMax, Order, ItemsVan);
                    break;
                default:
                    MessageBox.Show("Не существует такой таблицы", "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    break;
            }
        }
    
        public void Selection_Changed(object sender, SelectionChangedEventArgs e, DataGrid dg)
        {
            ComboBox box = sender as ComboBox;
            CurrentTable = Tables[box.SelectedIndex];
            Order = "ID DESC";
            GetItems(dg);
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

            /*if (e.Column.SortDirection == ListSortDirection.Descending)
            {
                e.Column.SortDirection = ListSortDirection.Ascending;
                sortDirection = "ASC";
            }
            else
            {
                e.Column.SortDirection = ListSortDirection.Descending;
                sortDirection = "DESC";
            }*/

            Order = $"{columnName}" + " " + sortDirection;

            GetItems();
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
            MessageBoxResult res = MessageBox.Show("Вы действительно хотите удалить строку?", "Подтвердите удаление", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.Yes) return;
           switch (CurrentTable)
            {
                case "PASSENGERS":
                    if (!Passenger.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        ItemsPassenger.Remove((Passenger)(sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "PAYMENTS":
                    if (!Payment.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        ItemsPayment.Remove((Payment)(sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "ROUTES":
                    if (!Route.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        ItemsRoute.Remove((Route)(sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "SCHEDULE":
                    if (!Schedule.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        ItemsSchedule.Remove((Schedule)(sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "STATIONS":
                    if (!Station.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        ItemsStation.Remove((Station)(sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "STATIONS_ROUTES":
                    if (!StationsRoute.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        ItemsStationsRoute.Remove((StationsRoute)(sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "TICKETS":
                    if (!Ticket.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        ItemsTicket.Remove((Ticket)(sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "TRAINS":
                    if (!Train.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        ItemsTrain.Remove((Train)(sender as DataGrid).SelectedCells[0].Item);
                    break;
                case "VANS":
                    if (!Van.Delete(sender, e, Conn))
                        MessageBox.Show("Ошибка при удалении строки");
                    else
                        ItemsVan.Remove((Van)(sender as DataGrid).SelectedCells[0].Item);
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
