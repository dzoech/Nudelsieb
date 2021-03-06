﻿using System.Threading.Tasks;

namespace Nudelsieb.Application.Notifications
{
    public interface IPushNotifyer
    {
        Task SubscribeAsync(DeviceInstallationDto installationRequest, string user);

        Task UnsubscribeAsync(string id, string user);

        Task<string> SendAsync(string message, string receiver);
    }
}
