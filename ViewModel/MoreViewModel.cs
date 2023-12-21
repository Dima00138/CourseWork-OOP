using System;
using CommunityToolkit.Mvvm.ComponentModel;

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
