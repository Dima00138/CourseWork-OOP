using CourseWork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CourseWork.Model
{
    public class TakeTicket
    {
        public int Id { get; set; }
        public int IdPassenger { get; set; }
        public int IdTrain { get; set; }
        public int IdVan { get; set; }
        public int SeatNumber { get; set; }
        public int FromWhere { get; set; }
        public int ToWhere { get; set; }
        public DateTime Date { get; set; }
        public int Cost { get; set; }
        public DateTime DatePay { get; set; }
        public char Status { get; set; }

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn, ObservableCollection<TakeTicket> Items)
        {
            var item = e.Row.DataContext as TakeTicket;
            if (item.Id == 0)
            {
                try
                {
                    string newVal = (e.EditingElement as TextBox).Text;
                    if (Items.IndexOf(item) == -1) { return false; }
                    switch (e.Column.Header.ToString()?.ToLower())
                    {
                        case "idpassenger":
                            Items[Items.IndexOf(item)].IdPassenger = Convert.ToInt32(newVal);
                            break;

                        case "idtrain":
                            Items[Items.IndexOf(item)].IdTrain = Convert.ToInt32(newVal);
                            break;
                        case "idvan":
                            Items[Items.IndexOf(item)].IdVan = Convert.ToInt32(newVal);
                            break;
                        case "seatnumber":
                            Items[Items.IndexOf(item)].SeatNumber = Convert.ToInt32(newVal);
                            break;
                        case "fromwhere":
                            Items[Items.IndexOf(item)].FromWhere = Convert.ToInt32(newVal);
                            break;
                        case "towhere":
                            Items[Items.IndexOf(item)].ToWhere = Convert.ToInt32(newVal);
                            break;
                        case "date":
                            DateTime date;
                            if (DateTime.TryParseExact(newVal, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                            {
                                Items[Items.IndexOf(item)].Date = date;
                            }
                            else throw new Exception();
                            break;
                        case "cost":
                            Items[Items.IndexOf(item)].Cost = Convert.ToInt32(newVal);
                            break;
                        case "datepay":
                            DateTime datepay;
                            if (DateTime.TryParseExact(newVal, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out datepay))
                            {
                                Items[Items.IndexOf(item)].DatePay = datepay;
                            }
                            else throw new Exception();
                            break;
                        case "status":
                            Items[Items.IndexOf(item)].Status = Convert.ToChar(newVal);
                            break;
                    }
                }
                catch 
                {
                    throw;
                }
                return true;
            }
            MessageBox.Show("Изменять заказы запрещено!", "Ошибка");
            return false;
        }

        public static bool Delete(object sender, KeyEventArgs e, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedCells[0].Item != null)
            {
                var item = dataGrid.SelectedCells[0].Item as TakeTicket;
                Repository<TakeTicket> Rep = new OrdersRepository(Conn);
                Rep.Delete(item, item.Id);
                return true;
            }
            return false;
        }

        public static bool Insert(object sender, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var item = dataGrid.SelectedCells[0].Item as TakeTicket;

            if (item.Id == 0)
            {
                if (!Checks.CheckOrder(item))
                {
                    MessageBox.Show("Ошибка валидации");
                    return false;
                }
                Repository<TakeTicket> Rep = new OrdersRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;
        }
    }
}
