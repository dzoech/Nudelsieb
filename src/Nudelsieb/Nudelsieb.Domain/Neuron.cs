using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nudelsieb.Domain
{
    public class Neuron
    {
        private List<Reminder> reminders = new List<Reminder>();

        public Guid Id { get; set; }

        public string Information { get; }

        public List<string> Groups { get; set; }

        public List<Reminder> Reminders
        {
            get => reminders; 
            set
            {
                foreach (var r in value)
                {
                    if (r.Subject != this)
                    {
                        throw new InvalidOperationException($"{nameof(Reminder)} is already assinged to another {nameof(Neuron)}");
                    }
                }

                reminders = value;
            }
        }

        public DateTimeOffset CreatedAt { get; }

        public Neuron(string information)
            : this(information, DateTimeOffset.UtcNow)
        {
        }

        public Neuron(string information, DateTimeOffset createdAt)
        {
            if (createdAt > DateTimeOffset.UtcNow)
            {
                throw new ArgumentException("Creation date must not be in the future.", nameof(createdAt));
            }

            Information = information;
            Groups = new List<string>();
            CreatedAt = createdAt;
        }

        public bool SetReminders(DateTimeOffset[] reminderTimes, out List<int> errorIndices)
        {
            var allRemindersSuccessful = true;
            errorIndices = new List<int>();

            for (int i = 0; i < reminderTimes.Length; i++)
            {
                var time = reminderTimes[i];

                if (time < DateTimeOffset.Now)
                {
                    allRemindersSuccessful = false;
                    errorIndices.Add(i);
                    continue;
                }

                var reminder = new Reminder(this)
                {
                    At = time,
                    State = ReminderState.Waiting
                };

                this.Reminders.Add(reminder);
            }

            return allRemindersSuccessful;
        }
    }
}
