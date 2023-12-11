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
        private readonly MainViewModel MainVM;
        public List<string> Stations { get; set; }
        public string FromStation { get; set; } = string.Empty;
        public string ToStation { get; set; } = string.Empty;
        public DateOnly DateBegin { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public TimeOnly DateEnd { get; set; }

        public RelayCommand SearchCommand { get; set; }

        private OracleContext conn;

        public HomeViewModel(MainViewModel mvm)
        {
            MainVM = mvm;
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
                    && DateEnd == TimeOnly.MinValue))
                    {
                        throw new Exception("Введите все данные для поиска");
                    }
                    StringBuilder Where = new StringBuilder();
                    Where.Append("ARRIVAL_POINT = '" + ToStation + "'");
                    Where.Append(" AND ");
                    Where.Append("DEPARTURE_POINT = '" + FromStation + "'");
                    Where.Append(" AND ");
                    if (!(DateBegin == DateOnly.MinValue
                    && DateEnd == TimeOnly.MinValue))
                        Where.Append("\"DATE\" >= " +
                           "TO_DATE('" + DateBegin + " " + DateEnd.ToString("HH:mm") + "', 'MM/DD/YYYY HH24:MI')");
                    else if (DateBegin == DateOnly.MinValue)
                        Where.Append("\"DATE\" >= " + "TO_DATE('" + DateEnd.ToString("HH:mm") + "', 'HH24:MI')");
                    else
                        Where.Append("\"DATE\" >= " + "TO_DATE('" + DateBegin + "', 'DD.MM.YYYY')");
                    MainVM.SearchVM = new SearchViewModel(Where.ToString());
                    MainVM.CurrentView = MainVM.SearchVM;
                    MainVM.PageName = "Search";
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message, "Ошибка поиска", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

    }
}
