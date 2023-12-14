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

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn)
        {
            var item = e.Row.DataContext as Van;
            if (e.Column.Header.ToString()?.ToLower() == "id")
            {
                MessageBox.Show("Редактировать Id нельзя");
                e.Cancel = true;
                return false;
            }
            if (item.Id == 0) return true;
            if (!Checks.CheckVan(item))
            {
                MessageBox.Show("Ошибка валидации");
                e.Cancel = true; return false;
            }
            string? col = e.Column.Header.ToString();
            string newVal;
            if (col.ToUpper() != "ISFREE") newVal = (e.EditingElement as TextBox).Text;
            else newVal = ((e.EditingElement as CheckBox).IsChecked == true) ? "1" : "0";
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
            var item = dataGrid.Items[dataGrid.Items.Count - 2] as Van;

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
