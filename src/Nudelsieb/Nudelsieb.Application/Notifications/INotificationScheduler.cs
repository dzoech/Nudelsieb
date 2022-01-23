using System;
using System.Threading.Tasks;

namespace Nudelsieb.Application.Notifications
{
    public interface INotificationScheduler
    {
        Task ScheduleAsync(string notification, Guid receiverUserId, DateTimeOffset at);
    }
}
