using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CourseWork.Services
{
    public class TimeOnlyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.TimeOnly timeOnly)
            {
                return timeOnly.ToString("hh:mm:ss tt", culture);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && TimeOnly.TryParse(stringValue, out var timeOnly))
            {
                return timeOnly;
            }

            return null;
        }
    }
}
