using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Cli.UserSettings
{
    public class UserSettingsModel
    {
        public class UserSettingsModelEndpoints
        {
            public EndpointSetting Braindump { get; set; } = new EndpointSetting("https://nudelsieb.zoechbauer.dev/braindump");
        }

        public class EndpointSetting
        {
            private Uri value;
            public Uri Previous { get; private set; }
            public Uri Value
            {
                get => value;
                set
                {
                    (Previous, this.value) = (this.value, value);
                    //Previous = this.value;
                    //this.value = value;
                }
            }

            public void Switch()
            {
                (value, Previous) = (Previous, value);
            }

            /// <summary>
            /// Required for JSON serializer.
            /// </summary>
            private EndpointSetting() : this("https://www.example.com")
            {
            }

            public EndpointSetting(string endpoint)
            {
                value = new Uri(endpoint);
                Previous = value;
            }
        }

        public UserSettingsModelEndpoints Endpoints { get; set; } = new UserSettingsModelEndpoints();

        public bool ConvertHashtagToGroup { get; set; } = false;
    }
}
