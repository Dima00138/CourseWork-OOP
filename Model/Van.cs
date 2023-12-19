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
    public class Van
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public int Capacity { get; set; }
        public bool IsFree { get; set; }

        public Van this[int i]
        {
            get { return this[i]; }
            set { this[i] = value; }
        }

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn, ObservableCollection<Van> Items)
        {
            var item = e.Row.DataContext as Van;
            string newVal;
            string? col = e.Column.Header.ToString();
            if (col.ToUpper() != "ISFREE") newVal = (e.EditingElement as TextBox).Text;
            else newVal = ((e.EditingElement as CheckBox).IsChecked == true) ? "1" : "0";
            if (e.Column.Header.ToString()?.ToLower() == "id")
            {
                MessageBox.Show("Редактировать Id нельзя");
                e.Cancel = true;
                return false;
            }
            try
            {

                if (Items.IndexOf(item) == -1) { return false; }
                switch (col.ToLower())
                {
                    case "type":
                        Items[Items.IndexOf(item)].Type = newVal;
                        break;
                    case "capacity":
                        Items[Items.IndexOf(item)].Capacity = Convert.ToInt32(newVal);
                        break;
                    case "isfree":
                        Items[Items.IndexOf(item)].IsFree = ((e.EditingElement as CheckBox).IsChecked == true);
                        break;
                }
            }
            catch
            {
                throw;
            }
            if (item?.Id == 0) return true;
            if (!Checks.CheckVan(item))
            {
                MessageBox.Show("Ошибка валидации");
                e.Cancel = true; return false;
            }
            Repository<Van> Rep = new VanRepository(Conn);
            Rep.Update(item, col, newVal);
            return true;
        }

        public static bool Delete(object sender, KeyEventArgs e, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedCells[0].Item != null)
            {
                var item = dataGrid.SelectedCells[0].Item as Van;
                Repository<Van> Rep = new VanRepository(Conn);
                Rep.Delete(item, item.Id);
                return true;
            }
            return false;
        }

        public static bool Insert(object sender, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var item = dataGrid.SelectedCells[0].Item as Van;

            if (item.Id == 0)
            {
                if (!Checks.CheckVan(item))
                {
                    MessageBox.Show("Ошибка валидации");
                    return false;
                }
                Repository<Van> Rep = new VanRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;
        }
    }
}
