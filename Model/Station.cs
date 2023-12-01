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
    public class Station
    {
        public int Id { get; set; }
        public string? StationName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn)
        {
            var item = e.Row.DataContext as Station;
            if (e.Column.Header.ToString()?.ToLower() == "id")
            {
                MessageBox.Show("Редактировать Id нельзя");
                e.Cancel = true;
                return false;
            }
            if (item.Id == 0) return true;
            string? col = e.Column.Header.ToString();
            string newVal = (e.EditingElement as TextBox).Text;
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
            var item = dataGrid.Items[dataGrid.Items.Count - 2] as Station;

            if (item.Id == 0)
            {
                if (!Checks.CheckStation(item))
                {
                    MessageBox.Show("Ошибка валидации");
                }
                Repository<Station> Rep = new StationRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;
        }
    }
}
