using System;
using System.Globalization;

namespace EnhancedShellExample.Infrastructure.Converters
{
    public class NotificationsConverter : ConverterBase
    {
        public override object OnValueReceived(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return value;
            }
            if (value is int)
            {
                if ((int)value > 99)
                    return "99+";
            }

            return value;
        }
    }
}
