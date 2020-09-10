using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Cli.UserSettings
{
    public class UserSettingsModel
    {
        public Uri BraindumpEndpoint { get; set; } = new Uri("https://nudelsieb.zoechbauer.dev");

        public bool ConvertHashtagToGroup { get; set; } = false;
    }
}
