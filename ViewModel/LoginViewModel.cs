using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CourseWork.ViewModel
{
    partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private object _loginView;

        public RelayCommand LoginControlCommand { get; set; }
        public RelayCommand LoginSettingsCommand {  get; set; }

        public LoginControlViewModel LoginControlVM { get; set; }
        public LoginSettingsViewModel LoginSettingsVM { get; set; }
        public RelayCommand CloseButtonCommand { get; set; }
        public RelayCommand HideButtonCommand { get; set; }

        public LoginViewModel()
        {
            LoginControlVM = new LoginControlViewModel(this);
            LoginSettingsVM = new LoginSettingsViewModel(this);
            
            LoginView = LoginControlVM;

            LoginControlCommand = new RelayCommand(() =>
            {
                LoginView = LoginControlVM;
            });

            LoginSettingsCommand = new RelayCommand(() =>
            {
                LoginView = LoginSettingsVM;
            });

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
    }
}
