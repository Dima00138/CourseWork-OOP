using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CourseWork.Model
{
    class DeprObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
