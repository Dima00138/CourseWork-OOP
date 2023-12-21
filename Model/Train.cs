using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using CourseWork.Services;
using System.Collections.ObjectModel;

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

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn, ObservableCollection<Train> Items)
        {
            var item = e.Row.DataContext as Train;
            string newVal;
            string? col = e.Column.Header.ToString();
            if (col?.ToUpper() != "ISFORPASSENGERS") newVal = (e.EditingElement as TextBox).Text;
            else newVal = ((e.EditingElement as CheckBox).IsChecked == true) ? "1" : "0";
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
                    case "categoryoftrain":
                        Items[Items.IndexOf(item)].CategoryOfTrain = newVal;
                        break;
                    case "isforpassengers":
                        Items[Items.IndexOf(item)].IsForPassengers = ((e.EditingElement as CheckBox).IsChecked == true);
                        break;
                    case "vans":
                        Items[Items.IndexOf(item)].Vans = newVal;
                        break;
                    case "parkingtime":
                        Items[Items.IndexOf(item)].ParkingTime = Convert.ToInt32(newVal);
                        break;
                    case "countofvans":
                        Items[Items.IndexOf(item)].CountOfVans = Convert.ToInt32(newVal);
                        break;
                }
            }
            catch
            {
                throw;
            }
            if (item?.Id == 0) return true;
            if (!Checks.CheckTrain(item))
            {
                MessageBox.Show("Ошибка валидации");
                e.Cancel = true; return false;
            }
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
            var item = dataGrid.SelectedCells[0].Item as Train;

            if (item.Id == 0)
            {
                if (!Checks.CheckTrain(item))
                {
                    MessageBox.Show("Ошибка валидации");
                    return false;
                }
                Repository<Train> Rep = new TrainRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;
        }

    }
}
