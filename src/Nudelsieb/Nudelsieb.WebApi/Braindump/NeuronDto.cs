using System;
using System.Collections.Generic;
using System.Linq;
using Nudelsieb.Domain.Aggregates.NeuronAggregate;

namespace Nudelsieb.WebApi.Braindump
{
    public class NeuronDto
    {
        public NeuronDto()
        { }

        public NeuronDto(Neuron neuron)
        {
            Information = neuron.Information;
            Id = neuron.Id;
            Groups = neuron.Groups;
            Reminders = neuron.Reminders.Select(r => r.At).ToList();
        }

        public Guid Id { get; set; }

        public string Information { get; set; } = string.Empty;

        public List<string> Groups { get; set; } = new List<string>();

        public List<DateTimeOffset> Reminders { get; set; } = new List<DateTimeOffset>();
    }
}
