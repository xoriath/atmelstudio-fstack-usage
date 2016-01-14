using System;
using System.Globalization;
using System.Windows.Data;

namespace StackUsageAnalyzer
{
    class FunctionStackInfoQualifierToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var q = value as FunctionStackInfo.Qualifier?;

            if (q == null || !q.HasValue)
                return "???";

            var qualifier = q.Value;

            if (qualifier.HasFlag(FunctionStackInfo.Qualifier.Static))
                return "Static";
            if (qualifier.HasFlag(FunctionStackInfo.Qualifier.Dynamic) && qualifier.HasFlag(FunctionStackInfo.Qualifier.Bounded))
                return "Dynamic, Bounded";
            if (qualifier.HasFlag(FunctionStackInfo.Qualifier.Dynamic))
                return "Dynamic, Unbounded";
            else
                return "???";

        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class FunctionStackInfoQualifierToTooltipStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var q = value as FunctionStackInfo.Qualifier?;

            if (q == null || !q.HasValue)
                return "Unknown";

            var qualifier = q.Value;

            if (qualifier.HasFlag(FunctionStackInfo.Qualifier.Static))
                return "Frame size for function is static. Stack usage number is reliable.";
            if (qualifier.HasFlag(FunctionStackInfo.Qualifier.Dynamic) && qualifier.HasFlag(FunctionStackInfo.Qualifier.Bounded))
                return "Frame size for function is not static, but the maximum stack usage is bounded. Stack usage number is a reliable maximum.";
            if (qualifier.HasFlag(FunctionStackInfo.Qualifier.Dynamic))
                return "Frame size for function is not static, and the maximum stack usage is not bounded. Stack usage number is not reliable.";
            else
                return "Unknown";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class FunctionStackInfoQualifierIsDynamicConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var q = value as FunctionStackInfo.Qualifier?;

            if (q == null || !q.HasValue)
                return false;

            var qualifier = q.Value;

            if (qualifier.HasFlag(FunctionStackInfo.Qualifier.Dynamic) && !qualifier.HasFlag(FunctionStackInfo.Qualifier.Bounded))
                return true;
            else
                return false;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class FunctionStackInfoQualifierIsDynamicAnBoundedConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var q = value as FunctionStackInfo.Qualifier?;

            if (q == null || !q.HasValue)
                return false;

            var qualifier = q.Value;

            if (qualifier.HasFlag(FunctionStackInfo.Qualifier.Dynamic) && qualifier.HasFlag(FunctionStackInfo.Qualifier.Bounded))
                return true;
            else
                return false;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
