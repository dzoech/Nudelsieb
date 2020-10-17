using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Nudelsieb.Cli.Parsers
{
    class ReminderParser : IReminderParser
    {
        private readonly CultureInfo cultureInfo;
        private readonly string decimalSeparator;
        private readonly Regex regex;

        public ReminderParser(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
            this.decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;
            // https://regexr.com/5dtn8
            // use string concatination instread of interpolation because of '{1}' regex quantifier
            regex = new Regex(@"^\s*(?<value>\d+(?:\" + decimalSeparator + @"\d+)?)\s*(?<unit>[smhdwMy]{1})\s*$");
        }

        /// <summary>
        /// </summary>
        /// <param name="reminder">Supported units are s, m, h, d, w, M, and y</param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public bool TryParse(string reminder, out TimeSpan timeSpan)
        {
            var m = regex.Match(reminder);

            if (m.Success == false) {
                timeSpan = default;
                return false;
            }

            var value = double.Parse(m.Groups["value"].Value);
            var unit = m.Groups["unit"].Value;

            timeSpan = unit switch
            {
                "s" => TimeSpan.FromSeconds(value),
                "m" => TimeSpan.FromMinutes(value),
                "h" => TimeSpan.FromHours(value),
                "d" => TimeSpan.FromDays(value),
                // we truncate because 1.5 w would result in a reminder with time of day + 12h:
                "w" => TimeSpan.FromDays(Math.Floor(value * 7)),
                "M" => TimeSpan.FromDays(Math.Floor(value * 30)), // todo depending on current date
                "Y" => TimeSpan.FromDays(Math.Floor(value * 365)),
                // should not match anyway:
                _ => throw new ArgumentOutOfRangeException($"Unit '{unit}' not supported.") 
            };

            return true;
        }
    }
}
