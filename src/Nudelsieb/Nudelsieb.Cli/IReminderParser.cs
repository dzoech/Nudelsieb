using System;

namespace Nudelsieb.Cli
{
    internal interface IReminderParser
    {
        bool TryParse(string reminder, out TimeSpan timeSpan);
    }
}