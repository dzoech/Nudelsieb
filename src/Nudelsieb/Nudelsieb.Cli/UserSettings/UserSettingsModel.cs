using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Cli.UserSettings
{
    public class UserSettingsModel
    {
        private static readonly Uri BlankUri = new Uri("about:blank");

        public class UserSettingsModelEndpoints
        {
            public EndpointSetting Braindump { get; set; } = new EndpointSetting();
        }

        public class EndpointSetting
        {
            public Uri Value { get; private set; } = BlankUri;
            public Uri Previous { get; private set; } = BlankUri;

            public void Switch()
            {
                (Value, Previous) = (Previous, Value);
            }

            public void Set(string endpoint)
            {
                Previous = Value;
                Value = new Uri(endpoint);
            }
        }

        public UserSettingsModelEndpoints Endpoints { get; set; } = new UserSettingsModelEndpoints();

        public bool ConvertHashtagToGroup { get; set; } = false;
    }
}
