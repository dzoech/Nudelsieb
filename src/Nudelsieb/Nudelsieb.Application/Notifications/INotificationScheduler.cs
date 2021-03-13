using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nudelsieb.Application.Notifications
{
    public interface INotificationScheduler
    {
        Task ScheduleAsync(string notification, DateTimeOffset at);
    }
}
