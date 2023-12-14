using CourseWork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

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

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn)
        {
            var item = e.Row.DataContext as TakeTicket;
            /*if (e.Column.Header.ToString()?.ToLower() == "id")
            {
                MessageBox.Show("Редактировать Id нельзя");
                e.Cancel = true;
                return false;
            }*/
            if (item.Id == 0) return true;
            /*if (!Checks.CheckOrder(item))
            {
                MessageBox.Show("Ошибка валидации");
                e.Cancel = true; return false;
            }*/
            MessageBox.Show("Изменять заказы запрещено!", "Ошибка");
            return false;
            /*string? col = e.Column.Header.ToString();
            string newVal = (e.EditingElement as TextBox).Text;
            Repository<TakeTicket> Rep = new OrdersRepository(Conn);
            Rep.Update(item, col, newVal);
            return true;*/
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
            var item = dataGrid.Items[dataGrid.Items.Count - 2] as TakeTicket;

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
