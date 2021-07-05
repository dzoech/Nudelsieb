using System;

namespace Nudelsieb.Persistence.Relational.Entities
{
    public enum ReminderState
    {
        Waiting,
        Active,
        Disabled
    }

    public class Reminder
    {
        public Guid Id { get; set; }

        public Guid SubjectId { get; set; }

        public Neuron Subject { get; set; }

        public DateTimeOffset At { get; set; }

        public ReminderState State { get; set; }
    }
}
