using CourseWork.Services;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace CourseWork.Model
{
    public class Route
    {
        public int Id { get; set; }
        public int DeparturePoint { get; set; }
        public int ArrivalPoint { get; set; }
        public int Distance { get; set; }
        public int Duration { get; set; }

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn, ObservableCollection<Route> Items)
        {
            var item = e.Row.DataContext as Route;
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
                    case "departurepoint":
                        Items[Items.IndexOf(item)].DeparturePoint = Convert.ToInt32(newVal);
                        break;
                    case "arrivalpoint":
                        Items[Items.IndexOf(item)].ArrivalPoint = Convert.ToInt32(newVal);
                        break;
                    case "distance":
                        Items[Items.IndexOf(item)].Distance = Convert.ToInt32(newVal);
                        break;
                    case "duration":
                        Items[Items.IndexOf(item)].Duration = Convert.ToInt32(newVal);
                        break;
                }
            }
            catch
            {
                throw;
            }
            if (item?.Id == 0) return true;
            if (!Checks.CheckRoute(item))
            {
                MessageBox.Show("Ошибка валидации");
                e.Cancel = true; return false;
            }
            Repository<Route> Rep = new RouteRepository(Conn);
            Rep.Update(item, col, newVal);
            return true;
        }

        public static bool Delete(object sender, KeyEventArgs e, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedCells[0].Item != null)
            {
                var item = dataGrid.SelectedCells[0].Item as Route;
                Repository<Route> Rep = new RouteRepository(Conn);
                Rep.Delete(item, item.Id);
                return true;
            }
            return false;
        }

        public static bool Insert(object sender, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var item = dataGrid.SelectedCells[0].Item as Route;

            if (item.Id == 0)
            {
                if (!Checks.CheckRoute(item))
                {
                    MessageBox.Show("Ошибка валидации");
                    return false;
                }
                Repository<Route> Rep = new RouteRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;

        }
    }
}
