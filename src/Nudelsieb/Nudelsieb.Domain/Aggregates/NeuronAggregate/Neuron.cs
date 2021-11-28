using System;
using System.Collections.Generic;

namespace Nudelsieb.Domain.Aggregates
{
    public class Neuron
    {
        public Neuron(string information)
            : this(information, DateTimeOffset.UtcNow)
        {
        }

        public Neuron(string information, DateTimeOffset createdAt)
        {
            if (createdAt > DateTimeOffset.UtcNow)
                throw new ArgumentException("Creation date must not be in the future.", nameof(createdAt));

            Information = information;
            Groups = new List<string>();
            CreatedAt = createdAt;
        }

        public Guid Id { get; set; }

        public string Information { get; }

        public List<string> Groups { get; set; }

        // TODO #DDD move logic into service layer
        //public List<Reminder> Reminders
        //{
        //    get => reminders;
        //    set
        //    {
        //        foreach (var r in value)
        //        {
        //            if (r.Subject != this)
        //                throw new InvalidOperationException($"{nameof(Reminder)} is already assinged to another {nameof(Neuron)}");
        //        }

        //        reminders = value;
        //    }
        //}

        public DateTimeOffset CreatedAt { get; set; }
    }
}
