using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nudelsieb.WebApi.Notifications
{
    public class DeviceInstallationDto
    {
        public string Id { get; set; } = string.Empty;

        public string PnsHandle { get; set; } = string.Empty;

        public string Platform { get; set; } = string.Empty;
    }
}
