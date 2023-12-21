using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using CourseWork.Services;
using System.Collections.ObjectModel;

namespace CourseWork.Model
{
    public class Station
    {
        public int Id { get; set; }
        public string? StationName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn, ObservableCollection<Station> Items)
        {
            var item = e.Row.DataContext as Station;
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
                    case "stationname":
                        Items[Items.IndexOf(item)].StationName = newVal;
                        break;
                    case "city":
                        Items[Items.IndexOf(item)].City = newVal;
                        break;
                    case "state":
                        Items[Items.IndexOf(item)].State = newVal;
                        break;
                    case "country":
                        Items[Items.IndexOf(item)].Country = newVal;
                        break;
                }
            }
            catch
            {
                throw;
            }
            if (item?.Id == 0) return true;
            if (!Checks.CheckStation(item))
            {
                MessageBox.Show("Ошибка валидации");
                e.Cancel = true; return false;
            }
            Repository<Station> Rep = new StationRepository(Conn);
            Rep.Update(item, col, newVal);
            return true;
        }

        public static bool Delete(object sender, KeyEventArgs e, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedCells[0].Item != null)
            {
                var item = dataGrid.SelectedCells[0].Item as Station;
                Repository<Station> Rep = new StationRepository(Conn);
                Rep.Delete(item, item.Id);
                return true;
            }
            return false;
        }

        public static bool Insert(object sender, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var item = dataGrid.SelectedCells[0].Item as Station;

            if (item.Id == 0)
            {
                if (!Checks.CheckStation(item))
                {
                    MessageBox.Show("Ошибка валидации");
                    return false;
                }
                Repository<Station> Rep = new StationRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;
        }
    }
}
