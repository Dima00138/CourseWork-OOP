using CourseWork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseWork.Model
{
    public class Passenger
    {
        public int Id { get; set; }
        public string? Passport { get; set; }
        public short Benefits { get; set; }
        public string? FullName { get; set; }

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn)
        {
            var item = e.Row.DataContext as Passenger;
            if (e.Column.Header.ToString()?.ToLower() == "id")
            {
                MessageBox.Show("Редактировать Id нельзя");
                e.Cancel = true;
                return false;
            }
            if (item.Id == 0) return true;
            if (!Checks.CheckPassenger(item))
            {
                MessageBox.Show("Ошибка валидации");
                e.Cancel = true; return false;
            }
            string? col = e.Column.Header.ToString();
            string newVal = (e.EditingElement as TextBox).Text;
            Repository<Passenger> Rep = new PassengerRepository(Conn);
            Rep.Update(item, col, newVal);
            return true;
        }

        public static bool Delete(object sender, KeyEventArgs e, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedCells[0].Item != null)
            {
                var item = dataGrid.SelectedCells[0].Item as Passenger;
                Repository<Passenger> Rep = new PassengerRepository(Conn);
                Rep.Delete(item, item.Id);
                return true;
            }
            return false;
        }

        public static bool Insert(object sender, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var item = dataGrid.Items[dataGrid.Items.Count - 2] as Passenger;

            if (item.Id == 0)
            {
                if (!Checks.CheckPassenger(item))
                {
                    MessageBox.Show("Ошибка валидации");
                    return false;
                }
                Repository<Passenger> Rep = new PassengerRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;
        }
    }
}
