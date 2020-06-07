using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Cli.Models
{
    class Neuron
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
