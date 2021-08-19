using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Nudelsieb.Mobile.Converters
{
    [Preserve(AllMembers = true)]
    public class DurationFromNowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTimeOffset dateTime))
            {
                return value;
            }

            var timeSpan = dateTime - DateTime.Now;
            var duration = timeSpan >= TimeSpan.Zero ? string.Empty : "-";

            if (timeSpan.Days != 0)
            {
                duration += timeSpan.ToString(@"%d'd '%h'h'");
            }
            else
            {
                duration += timeSpan.ToString(@"%h'h '%m'm'");
            }

            return duration;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
