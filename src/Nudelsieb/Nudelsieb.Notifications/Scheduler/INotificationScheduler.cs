using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nudelsieb.Notifications.Scheduler
{
    public interface INotificationScheduler
    {
        Task ScheduleAsync(string notification, DateTimeOffset at);
    }
}
