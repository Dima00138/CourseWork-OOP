using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseWork.Model;
using CourseWork.View;

namespace CourseWork.ViewModel
{
    class LoginViewModel : ObservableObject
    {
        public String LoginText { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public RelayCommand CloseButtonCommand { get; set; }
        public RelayCommand HideButtonCommand { get; set; }
        public RelayCommand SubmitButtonCommand { get; set; }
        public LoginViewModel()
        {
            LoginText = "Вход";
            Username = "";
            Password = "";

            SubmitButtonCommand = new RelayCommand(SubmitButton_Click);
            CloseButtonCommand = new RelayCommand(CloseButton_Click);
            HideButtonCommand = new RelayCommand(HideButton_Click);
        }

        public void CloseButton_Click()
        {
            Application.Current.Shutdown();
        }

        public void HideButton_Click()
        {
            Application.Current.MainWindow.WindowState
                = WindowState.Minimized;
        }

        public void SubmitButton_Click()
        {
            if (Username == "" && Password == "") { return; }

            try
            {

                //OracleContext conn = OracleContext.Create(Username, Password);
                OracleContext conn = OracleContext.Create("MANAGER", "MANAGER_PASS");


                MainWindow w = new MainWindow();

                w.Show();
                Application.Current.MainWindow.Close();
                Application.Current.MainWindow = w;
            }

            catch
            {
                MessageBox.Show("Неправильный логин или пароль",
                    "Ошибка соединения с базой данных");
            }

        }
    }
}
