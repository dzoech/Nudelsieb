using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nudelsieb.WebApi.Notifications
{
    public class AzureNotificationHubOptions
    {
        public const string SectionName = "AzureNotificationHub";
        public string ConnectionString { get; set; } = "";
        public string HubName { get; set; } = "";
    }
}
