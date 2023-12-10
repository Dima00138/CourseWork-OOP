using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using CourseWork.Services;

namespace CourseWork.Model
{
    public class Train
    {
        public int Id { get; set; }
        public string? CategoryOfTrain { get; set; }
        public bool IsForPassengers { get; set; }
        public string? Vans { get; set; }
        public int ParkingTime { get; set; }
        public int CountOfVans { get; set; }

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn)
        {
            var item = e.Row.DataContext as Train;
            if (e.Column.Header.ToString()?.ToLower() == "id")
            {
                MessageBox.Show("Редактировать Id нельзя");
                e.Cancel = true;
                return false;
            }
            if (item.Id == 0) return true;
            string? col = e.Column.Header.ToString();
            string newVal = "";
            if (col.ToUpper() != "ISFORPASSENGERS") newVal = (e.EditingElement as TextBox).Text;
            else newVal = ((e.EditingElement as CheckBox).IsChecked == true) ? "1" : "0";
            Repository<Train> Rep = new TrainRepository(Conn);
            Rep.Update(item, col, newVal);
            return true;
        }

        public static bool Delete(object sender, KeyEventArgs e, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedCells[0].Item != null)
            {
                var item = dataGrid.SelectedCells[0].Item as Train;
                Repository<Train> Rep = new TrainRepository(Conn);
                Rep.Delete(item, item.Id);
                return true;
            }
            return false;
        }

        public static bool Insert(object sender, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var item = dataGrid.Items[dataGrid.Items.Count - 2] as Train;

            if (item.Id == 0)
            {
                if (!Checks.CheckTrain(item))
                {
                    MessageBox.Show("Ошибка валидации");
                }
                Repository<Train> Rep = new TrainRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;
        }
    }
}
