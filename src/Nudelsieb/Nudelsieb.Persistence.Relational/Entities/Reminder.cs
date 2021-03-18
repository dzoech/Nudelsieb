using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Persistence.Relational.Entities
{
    public class Reminder
    {
        public Guid Id { get; set; }

        public Guid SubjectId { get; set; }

        public Neuron Subject { get; set; }

        public DateTimeOffset At { get; set; }

        public ReminderState State { get; set; }
    }

    public enum ReminderState
    {
        Waiting,
        Active,
        Disabled
    }
}
