using System;
using System.Threading.Tasks;
using Nudelsieb.Application.Notifications;
using Nudelsieb.Application.Persistence;
using Nudelsieb.Domain;

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

        public async Task<bool> ExecuteAsync(Guid neuronId, DateTimeOffset remindAt)
        {
            var neuron = await neuronRepository.GetByIdAsync(neuronId);
            var success = neuron.SetReminders(new[] { remindAt }, out var newReminders, out _);
            
            if (success)
            {
                await neuronRepository.AddRemindersAsync(newReminders);
                await scheduler.ScheduleAsync(neuron.Information, remindAt);
            }

            return success;
        }
    }
}
