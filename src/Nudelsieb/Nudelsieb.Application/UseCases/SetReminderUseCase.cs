using System;
using System.Threading.Tasks;
using Nudelsieb.Application.Notifications;
using Nudelsieb.Application.Persistence;

namespace Nudelsieb.Application.UseCases
{
    public class SetReminderUseCase : ISetReminderUseCase
    {
        private readonly INeuronRepository neuronRepository;
        private readonly INotificationScheduler scheduler;

        public SetReminderUseCase(INeuronRepository neuronRepository, INotificationScheduler scheduler)
        {
            this.neuronRepository = neuronRepository ?? throw new ArgumentNullException(nameof(neuronRepository));
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        public async Task<bool> ExecuteAsync(Guid neuronId, params DateTimeOffset[] remindAt)
        {
            var neuron = await neuronRepository.GetByIdAsync(neuronId);
            var success = neuron.SetReminders(remindAt, out var newReminders, out _);

            if (success)
            {
                await neuronRepository.AddRemindersAsync(newReminders);

                foreach (var r in newReminders)
                {
                    await scheduler.ScheduleAsync(neuron.Information, r.At);
                }
            }

            return success;
        }
    }
}
