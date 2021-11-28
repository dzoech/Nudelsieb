using System;

namespace Nudelsieb.Domain.Aggregates
{
    public class Reminder
    {
        public Reminder(DateTimeOffset at, Guid neuronReference)
        {
            if (at < DateTimeOffset.Now)
                throw new DomainException($"Reminder time '{at}' must not be in the past.");

            Id = Guid.NewGuid();
            At = at;
            State = ReminderState.Active;
            NeuronReference = neuronReference;
        }

        private Reminder()
        {
        }

        public Guid Id { get; private set; }

        public Guid NeuronReference { get; private set; }

        // TODO validate as in CLI
        public DateTimeOffset At { get; private set; }

        // TODO business logic
        public ReminderState State { get; private set; }

        /// <summary>
        /// Only used by the <see cref="IReminderRepository"/> to create an object of an already
        /// persisted domain entity.
        /// </summary>
        /// <remarks>
        /// Maybe this could be moved into the <see cref="IReminderRepository"/> so the setters
        /// could be made internal.
        /// </remarks>
        public static Reminder Reconstitute(Guid id, Guid neuronReference, DateTimeOffset at, ReminderState state)
        {
            return new Reminder
            {
                Id = id,
                NeuronReference = neuronReference,
                At = at,
                State = state
            };
        }
    }
}
