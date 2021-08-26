using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;

namespace Nudelsieb.Cli.Utils
{
    internal static class CommandLineUtilsConsoleExtensions
    {
        private const string ColumnDelimiter = "  ";
        private const int ColumnLengthLimit = 60;

        internal enum Highlighting
        {
            None = 0,
            Emphasize = 1,
            Warn = 2,
        }

        internal static void WriteLine(this IConsole console, Highlighting highlighting, object value)
        {
            Write(console, highlighting, value);
            console.WriteLine();
        }

        internal static void Write(this IConsole console, Highlighting highlighting, object value)
        {
            var fgColor = console.ForegroundColor;
            var bgColor = console.BackgroundColor;

            switch (highlighting)
            {
                case Highlighting.None:
                    console.ResetColor();
                    break;

                case Highlighting.Emphasize:
                    console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case Highlighting.Warn:
                    console.ForegroundColor = ConsoleColor.Red;
                    break;

                default:
                    break;
            }

            console.Write(value);
            console.ForegroundColor = fgColor;
            console.BackgroundColor = bgColor;
        }

        internal static void WriteTable<T>(this IConsole console, IEnumerable<T> data)
            where T : class
        {
            WriteTable(console, data, _ => Highlighting.None);
        }

        internal static void WriteTable<T>(this IConsole console, IEnumerable<T> data, Func<T, Highlighting> highlight)
            where T : class
        {
            WriteTable(console, data, r => r, highlight);
        }

        internal static void WriteTable<T, TProjection>(
            this IConsole console,
            IEnumerable<T> data,
            Func<T, TProjection> projection)
                where T : class
                where TProjection : class
        {
            WriteTable(console, data, projection, _ => Highlighting.None);
        }

        internal static void WriteTable<T, TProjection>(
            this IConsole console,
            IEnumerable<T> data,
            Func<T, TProjection> projection,
            Func<T, Highlighting> highlight)
                where T : class
                where TProjection : class
        {
            var projectionWithIndex = data.Select((val, idx) =>
                new
                {
                    Object = projection(val),
                    Index = idx
                });

            var propertyColumns = typeof(TProjection).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var maxColumnLengths = new int[propertyColumns.Length];
            var content = new string[data.Count(), propertyColumns.Length];
            var tableSize = (Rows: data.Count(), Columns: propertyColumns.Length);

            // Calculate padding iterate columns = properties
            for (int c = 0; c < tableSize.Columns; c++)
            {
                PropertyInfo prop = propertyColumns[c];
                var currMaxColumnLength = prop.Name.Length;

                // iterate rows
                foreach (var iter in projectionWithIndex)
                {
                    var text = prop.GetValue(iter.Object)?.ToString() ?? string.Empty;
                    content[iter.Index, c] = text;
                    currMaxColumnLength = Math.Max(currMaxColumnLength, text.Length);
                }

                maxColumnLengths[c] = Math.Min(currMaxColumnLength, ColumnLengthLimit);
            }

            // Print header
            var columnNames = string.Join(
                ColumnDelimiter,
                propertyColumns.Select((c, i) => AdjustToWidth(c.Name, (maxColumnLengths[i]))));

            var columnNameUnderlines = string.Join(
                ColumnDelimiter,
                maxColumnLengths.Select(l => new string('-', l)));

            console.WriteLine(columnNames);
            console.WriteLine(columnNameUnderlines);

            // Print content
            var item = data.GetEnumerator();
            for (int r = 0; r < tableSize.Rows; r++)
            {
                item.MoveNext();
                var rowHighlighting = highlight(item.Current);
                for (int c = 0; c < tableSize.Columns; c++)
                {
                    var paddedContent = AdjustToWidth(content[r, c], maxColumnLengths[c]);
                    console.Write(rowHighlighting, paddedContent);

                    if (c != tableSize.Columns)
                    {
                        console.Write(ColumnDelimiter);
                    }
                }

                console.WriteLine();
            }
        }

        private static string AdjustToWidth(string value, int width)
        {
            if (value.Length < width)
            {
                return value.PadRight(width);
            }

            if (value.Length > width)
            {
                return value.Remove(startIndex: width - 3) + "...";
            }

            return value;
        }
    }
}
