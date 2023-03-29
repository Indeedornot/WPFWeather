using System;
using System.Windows.Data;

namespace WPFWeather.Converters;

/// <summary>
/// Returns whether given parameter is equal bound value
/// </summary>
internal class StringToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return value is string v && parameter is string p && v == p;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is bool v)
        {
            return v ? parameter : Binding.DoNothing;
        }
        return Binding.DoNothing;
    }
}