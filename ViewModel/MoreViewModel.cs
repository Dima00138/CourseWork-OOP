using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;

namespace CourseWork.ViewModel
{
    class MoreViewModel : ObservableObject
    {
        public string Info { get; set; }

        public MoreViewModel() 
        {
            Info = "Приложение сделано Тимошенко Дмитрием Валерьевичем\n" +
                $"Copyright ©{DateTime.Now.ToString("yyyy")}";
        }

    }
}
