using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Shared.Clients.Notifications
{
    public class DeviceInstallation
    {
        public string Id { get; set; } = string.Empty;

        public string Platfrom { get; set; } = "fcm";

        public string PnsHandle { get; set; } = string.Empty;

        public ICollection<string> Tags { get; set; } = new List<string>();
    }
}
