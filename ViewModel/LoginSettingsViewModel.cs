using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseWork.Model;
using CourseWork.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseWork.ViewModel
{
    class LoginSettingsViewModel : ObservableObject
    {
        public String LoginText { get; set; }
        public String Hostname { get; set; }
        public String DbName { get; set; }
        public RelayCommand SubmitButtonCommand { get; set; }
        public RelayCommand ChangeSettingsCommand { get; set; }

        public LoginSettingsViewModel(LoginViewModel parent) 
        {
            LoginText = "Настройки БД";
            Hostname = "";
            DbName = "";

            SubmitButtonCommand = new RelayCommand(SubmitButton_Click);
            ChangeSettingsCommand = new RelayCommand(() =>
            {
                parent.LoginControlCommand.Execute(this);
            });
        }

        public void SubmitButton_Click()
        {
            if (Hostname == "" && DbName == "") { return; }

            try
            {
                Application.Current.Resources["connectionString"] = "Data Source = " + Hostname + "/" + DbName
                    + ";Persist Security Info=True;";
                ChangeSettingsCommand.Execute(this);
            }
            catch
            {

            }
        }
    }
}
