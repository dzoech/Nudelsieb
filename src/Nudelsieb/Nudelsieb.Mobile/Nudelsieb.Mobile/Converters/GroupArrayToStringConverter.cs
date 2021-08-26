using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Nudelsieb.Mobile.Converters
{
    internal class GroupArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string[] groups))
            {
                return value;
            }

            var prefixedGroups = groups.Select(g => $"#{g}");

            return string.Join(' ', prefixedGroups);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
