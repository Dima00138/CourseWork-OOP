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
                return dateOnly.ToString("d", culture);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && DateTime.TryParseExact(stringValue, "dd.MM.yyyy", culture, DateTimeStyles.None, out var dateTime))
            {
                return DateOnly.FromDateTime(dateTime);
            }

            return null;
        }
    }
}
