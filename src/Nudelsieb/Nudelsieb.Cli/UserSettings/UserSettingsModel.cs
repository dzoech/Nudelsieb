using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Cli.UserSettings
{
    public class UserSettingsModel
    {
        private static readonly Uri BlankUri = new Uri("https://localhost");

        public class UserSettingsModelEndpoints
        {
            public EndpointSetting Braindump { get; set; } = new EndpointSetting();
        }

        public class EndpointSetting
        {
            public Uri Value { get; set; } = BlankUri;
            public Uri Previous { get; set; } = BlankUri;
        }

        public UserSettingsModelEndpoints Endpoints { get; set; } = new UserSettingsModelEndpoints();

        public bool ConvertHashtagToGroup { get; set; } = false;
    }
}
