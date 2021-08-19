using System;
using System.Globalization;
using Nudelsieb.Mobile.RefitInternalGenerated;
using Xamarin.Forms;

namespace Nudelsieb.Mobile.Converters
{
    [Preserve(AllMembers = true)]
    public class ReminderTimeToGlyphConverter : IValueConverter
    {
        /// <param name="parameter">Bind the parenting label of the text that is converted.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTimeOffset dueAt))
                return value;

            var dueIn = dueAt - DateTimeOffset.Now;

            if (dueIn <= TimeSpan.Zero)
            {
                // reminder is already overdue
                Application.Current.Resources.TryGetValue("Highlight-900", out var retVal);
                ((Label)parameter).TextColor = (Color)retVal;
            }
            else if (dueIn <= TimeSpan.FromDays(1))
            {
                // reminder is due in near future (24 h)
                Application.Current.Resources.TryGetValue("Highlight-500", out var retVal);
                ((Label)parameter).TextColor = (Color)retVal;
            }
            else
            {
                ((Label)parameter).IsVisible = false;
                return null;
            }

            object text = null;

            ((Label)parameter)?.Resources.TryGetValue("New", out text);

            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
