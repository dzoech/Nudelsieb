using System;

namespace Nudelsieb.Persistence.Relational.Entities
{
    public class Group
    {
        public string Name { get; set; }

        public Guid NeuronId { get; set; }

        public Neuron Neuron { get; set; }

        public Guid UserId { get; set; }

    }
}
