using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFWeather.Converters;
public class InvertBooleanConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is bool bValue) {
            return !bValue;
        }

        throw new ArgumentException("Value is not a boolean", nameof(value));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is bool bValue) {
            return !bValue;
        }

        throw new ArgumentException("Value is not a boolean", nameof(value));
    }
}