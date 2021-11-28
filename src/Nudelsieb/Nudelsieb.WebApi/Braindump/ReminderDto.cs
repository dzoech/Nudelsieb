using System;
using System.Collections.Generic;
using Nudelsieb.Domain.Aggregates;

namespace Nudelsieb.WebApi.Braindump
{
    public class ReminderDto
    {
        public ReminderDto(Reminder reminder)
        {
            Id = reminder.Id;
            At = reminder.At;
            State = reminder.State;
        }

        public Guid Id { get; set; }

        public DateTimeOffset At { get; set; }

        public ReminderState State { get; set; }

        public string NeuronInformation { get; set; } = string.Empty;

        public List<string> NeuronGroups { get; set; } = new List<string>();
    }
}
