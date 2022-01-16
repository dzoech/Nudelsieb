using System;
using System.Collections.Generic;
using Nudelsieb.Domain.Aggregates;

namespace Nudelsieb.Application.UseCases
{
    public class EverythingDto
    {
        public List<Neuron> Neurons { get; internal set; } = new List<Neuron>();

        public List<Reminder> Reminders { get; internal set; } = new List<Reminder>();
    }
}
