using System;
using System.Globalization;
using System.Windows.Data;

namespace StackUsageAnalyzer
{
    class BytesToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as uint?;

            if (v == null || !v.HasValue)
                return "???";

            var i = v.Value;

            if (i < 1024f)
                return $"{ i } Bytes";
            else
                return $" { i / 1024f } KiB";

        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
