using System;

namespace Nudelsieb.Cli.Parsers
{
    internal interface IGroupParser
    {
        bool TryParse(string group, out string groupName);
    }
}