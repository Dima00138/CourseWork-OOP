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
using System.Globalization;
using System.Reflection;

namespace CourseWork.Model
{
    public class Schedule
    {
        public int Id { get; set; }
        public long? IdTrain { get; set; }
        public DateTime Date { get; set; }
        public long Route { get; set; }
        public string Frequency {  get; set; } = string.Empty;

        public void SetFrequency(short frequency)
        {
            switch (frequency)
            {
                case 1: Frequency = "Каждый день"; break;
                case 2: Frequency = "Каждый нечетный день"; break;
                case 3: Frequency = "Каждый четный день"; break;
                case 4: Frequency = "Единожды"; break;
            }
        }

        public short GetFrequency()
        {
            switch (Frequency)
            {
                case "Каждый день": return 1;
                case "Каждый нечетный день": return 2;
                case "Каждый четный день": return 3;
                case "Единожды": return 4;
            }
            return -1;
        }

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn, ObservableCollection<Schedule> Items)
        {
            var item = e.Row.DataContext as Schedule;
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
                    case "idtrain":
                        Items[Items.IndexOf(item)].IdTrain = Convert.ToInt32(newVal);
                        break;
                    case "route":
                        Items[Items.IndexOf(item)].Route = Convert.ToInt32(newVal);
                        break;
                    case "date":
                        DateTime date;
                        if (DateTime.TryParseExact(newVal, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            Items[Items.IndexOf(item)].Date = date;
                        }
                        else throw new Exception();
                        break;
                    case "frequency":
                        Items[Items.IndexOf(item)].Frequency = newVal;
                        break;
                }
            }
            catch
            {
                throw;
            }
            if (item?.Id == 0) return true;
            if (!Checks.CheckSchedule(item))
            {
                MessageBox.Show("Ошибка валидации");
                e.Cancel = true; return false;
            }
            Repository<Schedule> Rep = new ScheduleRepository(Conn);
            Rep.Update(item, col, newVal);
            return true;
        }

        public static bool Delete(object sender, KeyEventArgs e, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedCells[0].Item != null)
            {
                var item = dataGrid.SelectedCells[0].Item as Schedule;
                Repository<Schedule> Rep = new ScheduleRepository(Conn);
                Rep.Delete(item, item.Id);
                return true;
            }            
            return false;
        }

        public static bool Insert(object sender, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var item = dataGrid.SelectedCells[0].Item as Schedule;

            if (item.Id == 0)
            {
                if (!Checks.CheckSchedule(item))
                {
                    MessageBox.Show("Ошибка валидации");
                    return false;
                }
                Repository<Schedule> Rep = new ScheduleRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;
        }
    }
}
