using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using CourseWork.Services;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;

namespace CourseWork.Model
{
    public class StationsRoute
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int StationId { get; set; }
        public int StationOrder { get; set; }

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn, ObservableCollection<StationsRoute> Items)
        {
            var item = e.Row.DataContext as StationsRoute;
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
                    case "routeid":
                        Items[Items.IndexOf(item)].RouteId = Convert.ToInt32(newVal);
                        break;
                    case "stationid":
                        Items[Items.IndexOf(item)].StationId = Convert.ToInt32(newVal);
                        break;
                    case "stationorder":
                        Items[Items.IndexOf(item)].StationOrder = Convert.ToInt32(newVal);
                        break;
                }
            }
            catch
            {
                throw;
            }
            if (item?.Id == 0) return true;
            if (!Checks.CheckStationRoute(item))
            {
                MessageBox.Show("Ошибка валидации");
                e.Cancel = true; return false;
            }
            Repository<StationsRoute> Rep = new StationsRouteRepository(Conn);
            Rep.Update(item, col, newVal);
            return true;
        }

        public static bool Delete(object sender, KeyEventArgs e, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedCells[0].Item != null)
            {
                var item = dataGrid.SelectedCells[0].Item as StationsRoute;
                Repository<StationsRoute> Rep = new StationsRouteRepository(Conn);
                Rep.Delete(item, item.Id);
                return true;
            }
            return false;
        }

        public static bool Insert(object sender, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var item = dataGrid.SelectedCells[0].Item as StationsRoute;

            if (item.Id == 0)
            {
                if (!Checks.CheckStationRoute(item))
                {
                    MessageBox.Show("Ошибка валидации");
                    return false;
                }
                Repository<StationsRoute> Rep = new StationsRouteRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;
        }
    }
}
