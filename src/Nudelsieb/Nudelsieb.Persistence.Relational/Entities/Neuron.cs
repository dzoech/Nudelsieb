using System;
using System.Collections.Generic;

namespace Nudelsieb.Persistence.Relational.Entities
{
    public class Neuron
    {
        public Guid Id { get; set; }

        public string Information { get; set; }

        public ICollection<Group> Groups { get; set; } = new List<Group>();

        public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

        public DateTimeOffset CreatedAt { get; set; }

        public Guid UserId { get; set; }

    }
}
