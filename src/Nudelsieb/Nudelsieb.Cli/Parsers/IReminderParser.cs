using System;

namespace Nudelsieb.Cli.Parsers
{
    internal interface IReminderParser
    {
        bool TryParse(string reminder, out TimeSpan timeSpan);
    }
}