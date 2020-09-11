using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Cli.UserSettings
{
    public class UserSettingsModel
    {

        public class UserSettingsModelEndpoints
        {
            public Uri Braindump { get; set; } = new Uri("https://nudelsieb.zoechbauer.dev");
        }

        public UserSettingsModelEndpoints Endpoints { get; set; } = new UserSettingsModelEndpoints();

        public bool ConvertHashtagToGroup { get; set; } = false;
    }
}
