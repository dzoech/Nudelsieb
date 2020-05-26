using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nudelsieb.Cli.Services
{
    class BraindumService : IBraindumpService
    {
        private readonly IConsole console;

        public BraindumService(IConsole console)
        {
            this.console = console;
        }

        public async Task Add(Neuron neuron)
        {
            this.console.WriteLine($"Adding {neuron.Information} with {neuron.Groups.Count} groups: '{string.Join(", ", neuron.Groups)}'");
        }

        public List<Neuron> GetAll()
        {
            return new List<Neuron>();
        }
    }
}
