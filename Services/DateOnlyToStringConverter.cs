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
            if (value is string string1Value && DateTime.TryParseExact(string1Value, "MM/d/yyyy", culture, DateTimeStyles.None, out var dateTime1))
            {
                return DateOnly.FromDateTime(dateTime1);
            }
            if (value is string stringValue2 && DateTime.TryParseExact(stringValue2, "M/dd/yyyy", culture, DateTimeStyles.None, out var dateTime2))
            {
                return DateOnly.FromDateTime(dateTime2);
            }
            if (value is string stringValue3 && DateTime.TryParseExact(stringValue3, "M/d/yyyy", culture, DateTimeStyles.None, out var dateTime3))
            {
                return DateOnly.FromDateTime(dateTime3);
            }

            return null;
        }
    }
}
