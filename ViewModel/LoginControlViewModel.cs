using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseWork.Model;
using CourseWork.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseWork.ViewModel
{
    class LoginControlViewModel : ObservableObject
    {
        public String LoginText { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public RelayCommand SubmitButtonCommand { get; set; }
        public RelayCommand ChangeSettingsCommand { get; set; }

        public LoginControlViewModel(LoginViewModel parent) 
        {
            LoginText = "Вход";
            Username = "";
            Password = "";

            SubmitButtonCommand = new RelayCommand(SubmitButton_Click);
            ChangeSettingsCommand = new RelayCommand(() =>
            {
                parent.LoginSettingsCommand.Execute(this);
            });
        }

        public void SubmitButton_Click()
        {
            if (Username == "" && Password == "") { return; }

            try
            {

                OracleContext conn = OracleContext.Create(Username, Password);
                //OracleContext conn = OracleContext.Create("USERS", "USERS_PASS");
                //OracleContext conn = OracleContext.Create("MANAGER", "MANAGER_PASS");


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
