using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger<BraindumService> logger;

        public BraindumService(ILogger<BraindumService> logger)
        {
            this.logger = logger;
        }

        public async Task Add(Neuron neuron)
        {
            this.logger.LogDebug(
                $"Adding '{neuron.Information}' " +
                $"with {neuron.Groups.Count} groups: '{string.Join(", ", neuron.Groups)}'");

            this.logger.LogInformation("Info");
        }

        public List<Neuron> GetAll()
        {
            return new List<Neuron>();
        }
    }
}
