using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nudelsieb.WebApi.Braindump
{
    public class NeuronDto
    {
        public Guid Id { get; set; }

        public string Information { get; set; } = string.Empty;

        public List<string> Groups { get; set; } = new List<string>();
    }
}
