using CommunityToolkit.Mvvm.Input;
using CourseWork.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseWork.ViewModel
{
    internal class HomeViewModel
    {
        public List<string> Stations { get; set; }
        public string FromStation { get; set; } = string.Empty;
        public string ToStation { get; set; } = string.Empty;
        public DateOnly DateBegin { get; set; } = DateOnly.MinValue;
        public DateOnly DateEnd { get; set; } = DateOnly.MinValue;

        public RelayCommand SearchCommand { get; set; }

        private OracleContext conn;

        public HomeViewModel()
        {
            Stations = new List<string>();
            conn = OracleContext.Create();

            string sql = "SELECT STATION_NAME FROM MANAGER.STATIONS";

            OracleCommand cmd = new OracleCommand(sql);
            using (OracleDataReader reader = conn.SelectQuery(cmd))
            {
                while (reader.Read())
                {
                    Stations.Add(reader.GetString(0));
                }
                conn.conn.Close();
            }

            SearchCommand = new RelayCommand(() =>
            {
                try
                {
                    if (ToStation.Trim() == ""
                    && FromStation.Trim() == ""
                    && (DateBegin == DateOnly.MinValue
                    || DateEnd == DateOnly.MinValue))
                    {
                        throw new Exception("Введите все данные для поиска");
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message, "Ошибка поиска", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

    }
}
