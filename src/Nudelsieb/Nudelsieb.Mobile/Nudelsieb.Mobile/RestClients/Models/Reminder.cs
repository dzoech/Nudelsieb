using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Mobile.RestClients.Models
{
    public class Reminder
    {
        public Guid Id { get; set; }

        public DateTimeOffset At { get; set; }

        public string NeuronInformation { get; set; } = string.Empty;

        public string[] NeuronGroups { get; set; } = Array.Empty<string>();

        public bool IsOverdue => this.At < DateTimeOffset.Now;
    }
}
