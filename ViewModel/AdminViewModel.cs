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
            Items = new ObservableCollection<object>();

            string sql = $"SELECT * FROM MANAGER.{CurrentTable} WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
            

            using (OracleDataReader reader = Conn.SelectQuery(new OracleCommand(sql)))
            {
                while (reader.Read())
                {
                    switch (CurrentTable)
                    {
                        case "PASSENGERS":
                            Passenger itemPas = new Passenger();
                            itemPas.Id = Convert.ToInt32(reader["ID"]);
                            itemPas.FullName = reader["FULL_NAME"].ToString();
                            itemPas.Passport = reader["PASSPORT"].ToString();
                            itemPas.Benefits = Convert.ToInt16(reader["BENEFITS"]);
                            Items.Add(itemPas);
                            break;
                        case "PAYMENTS":
                            Payment itemPayment = new Payment();
                            itemPayment.Id = Convert.ToInt32(reader["ID"]);
                            itemPayment.IdTicket = Convert.ToInt32(reader["ID_TICKET"]);
                            itemPayment.DatePay = Convert.ToDateTime(reader["DATE_PAY"]);
                            itemPayment.Status = Convert.ToChar(reader["STATUS"]);
                            Items.Add(itemPayment);
                            break;
                        case "ROUTES":
                            Route itemRoute = new Route();
                            itemRoute.Id = Convert.ToInt32(reader["ID"]);
                            itemRoute.DeparturePoint = Convert.ToInt32(reader["DEPARTURE_POINT"]);
                            itemRoute.ArrivalPoint = Convert.ToInt32(reader["ARRIVAL_POINT"]);
                            itemRoute.Distance = Convert.ToInt32(reader["DISTANCE"]);
                            itemRoute.Duration = Convert.ToInt32(reader["DURATION"]);
                            Items.Add(itemRoute);
                            break;
                        case "SCHEDULE":
                            if (reader.GetBoolean(11) == false) continue;
                            Schedule itemSche = new Schedule();
                            itemSche.Id = reader.GetInt64(0);
                            itemSche.IdTrain = reader.GetInt64(1);
                            itemSche.Route = reader.GetInt64(3);
                            itemSche.Date = reader.GetDateTime(2);
                            itemSche.SetFrequency(reader.GetInt16(4)); break;
                        case "STATIONS":
                            Station itemStation = new Station();
                            itemStation.Id = Convert.ToInt32(reader["ID"]);
                            itemStation.StationName = reader["STATION_NAME"].ToString();
                            itemStation.City = reader["CITY"].ToString();
                            itemStation.State = reader["STATE"].ToString();
                            itemStation.Country = reader["COUNTRY"].ToString();
                            Items.Add(itemStation);
                            break;
                        case "STATIONS_ROUTES":
                            StationsRoute itemStationRoute = new StationsRoute();
                            itemStationRoute.Id = Convert.ToInt32(reader["ID"]);
                            itemStationRoute.RouteId = Convert.ToInt32(reader["ROUTE_ID"]);
                            itemStationRoute.StationId = Convert.ToInt32(reader["STATION_ID"]);
                            itemStationRoute.StationOrder = Convert.ToInt32(reader["STATION_ORDER"]);
                            Items.Add(itemStationRoute);
                            break;
                        case "TICKETS":
                            Ticket itemTic = new Ticket();
                            itemTic.Id = Convert.ToInt32(reader["ID"]);
                            itemTic.IdPassenger = Convert.ToInt32(reader["ID_PASSENGER"]);
                            itemTic.IdTrain = Convert.ToInt32(reader["ID_TRAIN"]);
                            itemTic.IdVan = Convert.ToInt32(reader["ID_VAN"]);
                            itemTic.SeatNumber = Convert.ToInt32(reader["SEAT_NUMBER"]);
                            itemTic.FromWhere = Convert.ToInt32(reader["FROM_WHERE"]);
                            itemTic.ToWhere = Convert.ToInt32(reader["TO_WHERE"]);
                            itemTic.Date = Convert.ToDateTime(reader["DATE"]);
                            itemTic.Cost = Convert.ToInt32(reader["COST"]);
                            Items.Add(itemTic);
                            break;
                        case "TRAINS":
                            Train itemTra = new Train();
                            itemTra.Id = Convert.ToInt32(reader["ID"]);
                            itemTra.CategoryOfTrain = reader["CATEGORY_OF_TRAIN"].ToString();
                            itemTra.IsForPassengers = Convert.ToBoolean(reader["IS_FOR_PASSENGERS"]);
                            itemTra.Vans = reader["VANS"].ToString();
                            itemTra.CountOfVans = Convert.ToInt32(reader["COUNT_OF_VANS"]);
                            itemTra.ParkingTime = Convert.ToInt32(reader["PARKING_TIME"]);
                            Items.Add(itemTra);
                            break;
                        case "VANS":
                            Van itemVan = new Van();
                            itemVan.Id = Convert.ToInt32(reader["ID"]);
                            itemVan.Type = reader["TYPE"].ToString();
                            itemVan.Capacity = Convert.ToInt32(reader["CAPACITY"]);
                            itemVan.IsFree = Convert.ToBoolean(reader["IS_FREE"]);
                            Items.Add(itemVan);
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
    }
}
