using System;
using System.Collections.Generic;
using System.Linq;
using Nudelsieb.Domain.Aggregates.NeuronAggregate;

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

            Id = Guid.NewGuid();
            Information = information;
            Groups = new List<Group>();
            CreatedAt = createdAt;
        }

        private Neuron()
        {
        }

        public Guid Id { get; private set; }

        public string Information { get; private set; } = string.Empty;

        public List<Group> Groups { get; private set; } = new List<Group>();

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

        public DateTimeOffset CreatedAt { get; private set; }

        /// <summary>
        /// Only used by the <see cref="INeuronRepository"/> to create an object of an already
        /// persisted domain entity.
        /// </summary>
        /// <remarks>
        /// Maybe this could be moved into the <see cref="INeuronRepository"/> and the setters could
        /// be made internal.
        /// </remarks>
        public static Neuron Reconstitute(Guid id, string information, List<string> groupNames, DateTimeOffset createdAt)
        {
            return new Neuron
            {
                Id = id,
                Information = information,
                Groups = groupNames.Select(n => new Group(n)).ToList(),
                CreatedAt = createdAt
            };
        }

        public void AssignToGroup(Group group)
        {
            Groups.Add(group);
        }
    }
}
