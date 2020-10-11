using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nudelsieb.Domain
{
    public class Neuron
    {
        public Guid Id { get; set; }

        public string Information { get; set; }

        public List<string> Groups { get; set; }

        public List<Reminder> Reminders { get; }

        public Neuron(string information)
        {
            Information = information;
            Groups = new List<string>();
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

                var reminder = new Reminder
                {
                    At = time,
                    State = ReminderState.Waiting,
                    Subject = this
                };

                Reminders.Add(reminder);
            }

            return allRemindersSuccessful;
        }
    }
}
