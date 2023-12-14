using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CourseWork.Services
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                // Здесь вы можете задать формат даты, который вы хотите использовать
                return dateTime.ToString("dd/MM/yyyy hh:mm tt");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && DateTime.TryParseExact(stringValue, "dd/MM/yyyy hh:mm tt", culture, DateTimeStyles.None, out var dateTime))
            {
                return dateTime;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
