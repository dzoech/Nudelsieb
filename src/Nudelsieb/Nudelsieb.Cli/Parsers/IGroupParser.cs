using System;

namespace Nudelsieb.Cli.Parsers
{
    internal interface IGroupParser
    {
        public string ErrorMessage { get; }
        bool TryParse(string group, out string groupName);
    }
}