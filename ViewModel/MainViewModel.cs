using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseWork.Model;
using CourseWork.View;
using System;
using System.Windows;

namespace CourseWork.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private object _currentView;
        private string _pageName;

        public bool IsAdmin { get; set; }

        public string PageName
        {
            get { return _pageName; }
            set
            {
                _pageName = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LoginViewCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand BoardViewCommand { get; set; }
        public RelayCommand AdminViewCommand { get; set; }
        public RelayCommand OrdersViewCommand { get; set; }
        public RelayCommand MoreViewCommand { get; set; }


        public RelayCommand CloseButtonCommand { get; set; }
        public RelayCommand HideButtonCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public BoardViewModel BoardVM { get; set; }
        public SearchViewModel SearchVM { get; set; }
        public AdminViewModel AdminVM { get; set; }
        public OrdersViewModel OrdersVM { get; set; }
        public MoreViewModel MoreVM { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            PageName = "Home";

            HomeVM = new HomeViewModel(this);
            BoardVM = new BoardViewModel();
            OrdersVM = new OrdersViewModel();
            MoreVM = new MoreViewModel();
            AdminVM = new AdminViewModel();
            SearchVM = null;

            CurrentView = HomeVM;

            IsAdmin = OracleContext.Create().IsAdmin;

            LoginViewCommand = new RelayCommand(() =>
            {
                try
                { 
                CurrentView = HomeVM;
                PageName = "Home";

                OracleContext.Delete();
                Login w = new Login();

                w.Show();
                Application.Current.MainWindow.Close();
                Application.Current.MainWindow = w;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            HomeViewCommand = new RelayCommand(() =>
            {
                try
                {
                HomeVM = new HomeViewModel(this);
                CurrentView = HomeVM;
                PageName = "Home";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            BoardViewCommand = new RelayCommand(() =>
            {
                try
                {
                    BoardVM = new BoardViewModel();
                    CurrentView = BoardVM;
                    PageName = "Board";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            OrdersViewCommand = new RelayCommand(() =>
            {
                try 
                {
                OrdersVM = new OrdersViewModel();
                CurrentView = OrdersVM;
                PageName = "Orders";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            MoreViewCommand = new RelayCommand(() =>
            {
                try
                {
                    MoreVM = new MoreViewModel();
                    CurrentView = MoreVM;
                    PageName = "More";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            AdminViewCommand = new RelayCommand(() =>
            {
                try
                {
                    CurrentView = AdminVM;
                    PageName = "Admin";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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
