using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Oracle.ManagedDataAccess.Client;

namespace CourseWork.Model
{
    public class OracleContext
    {
        private static OracleContext? Instance;
        public OracleConnection conn;
        public bool IsAdmin { get; set; } = false;

        private OracleContext(string Username, string Password)
        {
            try
            {
                conn = new OracleConnection(
                        ConfigurationManager.ConnectionStrings["OracleDbContext"] +
                        $"User Id = {Username}; Password = {Password};"
                    );
                conn.KeepAlive = true;
                conn.Open();
                conn.Close();
                if (Username == "MANAGER")
                {
                    IsAdmin = true;
                }

            }
            catch
            {
                Exception exception = new Exception();
                throw exception;
            }
        }


        public static OracleContext Create(string Username = "", string Password = "")
        {
            if (Instance == null)
            {
                Instance = new OracleContext(Username, Password);
            }
            return Instance;
        }
        public static void Delete()
        {
            Instance = null;
        }
    }
}
