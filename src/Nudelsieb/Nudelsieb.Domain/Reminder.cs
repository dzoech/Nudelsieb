using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Domain
{
    public class Reminder
    {
        public Guid Id { get; set; }

        public Neuron Subject { get; }

        public DateTimeOffset At { get; set; } = DateTimeOffset.Now;

        public ReminderState State { get; set; }

        public Reminder(Neuron subject)
        {
            Subject = subject;
        }
    }
}
