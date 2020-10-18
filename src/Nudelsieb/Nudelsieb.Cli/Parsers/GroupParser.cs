using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Nudelsieb.Cli.Parsers
{
    class GroupParser : IGroupParser
    {
        public string ErrorMessage => "Group name must only consist of alphanumeric characters, digits, and dashes/hyphens (-).";

        private const string AlphanumericDashUnderscoreDigitRegex = @"^[\w\d-]+$";

        private Regex regex = new Regex(AlphanumericDashUnderscoreDigitRegex);

        public bool TryParse(string group, out string groupName)
        {
            groupName = group
                .Trim()
                .TrimStart('#')
                .Replace(' ', '-');

            return regex.IsMatch(groupName);
        }
    }
}
