using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nudelsieb.Application.Notifications;
using Nudelsieb.Domain;
using Nudelsieb.Domain.Aggregates;

namespace Nudelsieb.Application.UseCases
{
    public class SetRemindersUseCase
    {
        private readonly INeuronRepository neuronRepository;
        private readonly IReminderRepository reminderRepository;
        private readonly INotificationScheduler notificationScheduler;
        private readonly ILogger logger;

        public SetRemindersUseCase(
            INeuronRepository neuronRepository,
            INotificationScheduler scheduler,
            IReminderRepository reminderRepository,
            ILogger<SetRemindersUseCase> logger)
        {
            this.neuronRepository = neuronRepository ?? throw new ArgumentNullException(nameof(neuronRepository));
            this.notificationScheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            this.reminderRepository = reminderRepository ?? throw new ArgumentNullException(nameof(reminderRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<(bool success, IEnumerable<DateTimeOffset> faultyReminders)> ExecuteAsync(
            Guid neuronId, Guid receiverUserId, params DateTimeOffset[] remindAt)
        {
            var faultyReminders = new List<DateTimeOffset>();
            var neuron = await neuronRepository.GetByIdAsync(neuronId);

            foreach (var at in remindAt)
            {
                try
                {
                    var reminder = new Reminder(at, neuron.Id);
                    await reminderRepository.AddAsync(reminder);
                    await notificationScheduler.ScheduleAsync(neuron.Information, receiverUserId, reminder.At);
                }
                catch (DomainException ex)
                {
                    logger.LogInformation(ex, "Error creating reminder.");
                    faultyReminders.Add(at);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error creating reminder.");
                    faultyReminders.Add(at);
                }
            }

            bool success = faultyReminders.Count == 0;

            return (success, faultyReminders);
        }
    }
}
