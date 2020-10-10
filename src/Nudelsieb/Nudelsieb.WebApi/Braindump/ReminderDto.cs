using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nudelsieb.WebApi.Braindump
{
    public class ReminderDto
    {
        public Guid Id { get; set; }

        public DateTimeOffset At { get; set; }

        public ReminderState MyProperty { get; set; }

        public string NeuronInformation { get; set; } = string.Empty;

        public List<string> NeuronGroups { get; set; } = new List<string>();
    }
}
