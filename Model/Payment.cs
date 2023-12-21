using CourseWork.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseWork.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public int IdTicket { get; set; }
        public DateTime DatePay { get; set; }
        public char Status { get; set; }

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn, ObservableCollection<Payment> Items)
        {
            var item = e.Row.DataContext as Payment;
            string? col = e.Column.Header.ToString();
            string newVal = (e.EditingElement as TextBox).Text;
            if (col?.ToLower() == "id")
            {
                MessageBox.Show("Редактировать Id нельзя");
                e.Cancel = true;
                return false;
            }
            try
            {
                if (Items.IndexOf(item) == -1) { return false; }
                switch (col?.ToLower())
                {
                    case "idticket":
                        Items[Items.IndexOf(item)].IdTicket = Convert.ToInt32(newVal);
                        break;
                    case "datepay":
                        DateTime date;
                        if (DateTime.TryParseExact(newVal, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            Items[Items.IndexOf(item)].DatePay = date;
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
            if (item?.Id == 0) return true;
            if (!Checks.CheckPayment(item))
            {
                MessageBox.Show("Ошибка валидации");
                e.Cancel = true; return false;
            }
            Repository<Payment> Rep = new PaymentRepository(Conn);
            Rep.Update(item, col, newVal);
            return true;
        }

        public static bool Delete(object sender, KeyEventArgs e, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedCells[0].Item != null)
            {
                var item = dataGrid.SelectedCells[0].Item as Payment;
                Repository<Payment> Rep = new PaymentRepository(Conn);
                Rep.Delete(item, item.Id);
                return true;
            }            
            return false;
        }

        public static bool Insert(object sender, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var item = dataGrid.SelectedCells[0].Item as Payment;

            if (item.Id == 0)
            {
                if (!Checks.CheckPayment(item))
                {
                    MessageBox.Show("Ошибка валидации");
                    return false;
                }
                Repository<Payment> Rep = new PaymentRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;
        }
    }
}
