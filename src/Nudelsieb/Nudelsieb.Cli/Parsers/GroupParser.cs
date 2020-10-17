using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Nudelsieb.Cli.Parsers
{
    class GroupParser : IGroupParser
    {
        public const string AlphanumericDashUnderscoreDigitRegex = @"^[\w\d-]+$";

        public const string ErrorMessage = "Group name must only consist of alphanumeric characters, digits, and dashes/hyphens (-).";

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
