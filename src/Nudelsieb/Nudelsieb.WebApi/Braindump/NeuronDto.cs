using System;
using System.Collections.Generic;
using System.Linq;
using Nudelsieb.Domain.Aggregates;

namespace Nudelsieb.WebApi.Braindump
{
    public class NeuronDto
    {
        public NeuronDto()
        { }

        [Obsolete("manually set property values", error: true)]
        public NeuronDto(Neuron neuron)
        {
            Information = neuron.Information;
            Id = neuron.Id;
            Groups = neuron.Groups.Select(g => g.Name).ToList();
        }

        public Guid Id { get; set; }

        public string Information { get; set; } = string.Empty;

        public List<string> Groups { get; set; } = new List<string>();

        public List<DateTimeOffset> Reminders { get; set; } = new List<DateTimeOffset>();
    }
}
