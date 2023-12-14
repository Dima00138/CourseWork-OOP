using System;
using System.Globalization;
using System.Windows.Data;

namespace CourseWork.Services
{
    public class DateOnlyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.DateOnly dateOnly)
            {
                return dateOnly.ToString("MM/dd/yyyy");
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && DateTime.TryParseExact(stringValue, "MM/dd/yyyy", culture, DateTimeStyles.None, out var dateTime))
            {
                return DateOnly.FromDateTime(dateTime);
            }

            return null;
        }
    }
}
