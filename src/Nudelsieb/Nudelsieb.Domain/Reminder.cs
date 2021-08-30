using System;
using Nudelsieb.Domain.Aggregates;

namespace Nudelsieb.Domain
{
    public class Reminder
    {
        public Reminder(Neuron subject)
        {
            Subject = subject;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Neuron Subject { get; }

        // TODO validate as in CLI
        public DateTimeOffset At { get; set; } = DateTimeOffset.Now;

        // TODO business logic
        public ReminderState State { get; set; }
    }
}
