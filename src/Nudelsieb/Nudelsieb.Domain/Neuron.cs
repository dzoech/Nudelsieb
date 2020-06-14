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

        public Neuron(string information)
        {
            Information = information;
            Groups = new List<string>();
        }
    }
}
