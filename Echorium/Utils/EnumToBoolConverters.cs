using Avalonia.Data;
using Avalonia.Data.Converters;
using System;

namespace Echorium.Utils
{
    public class EnumBooleanConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter is not string parameterString)
                return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

            if (Enum.IsDefined(value.GetType(), value) == false)
                return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

            object parameterValue = Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value);
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter is not string parameterString)
                return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

            return Enum.Parse(targetType, parameterString);
        }
    }
}
