using System.ComponentModel.DataAnnotations;

namespace Nudelsieb.Persistence.Relational.Entities
{
    public class Group
    {
        public string Name { get; set; }

        public Neuron Neuron { get; set; }
    }
}