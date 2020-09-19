using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Nudelsieb.Cli.Models;
using Nudelsieb.Cli.RestClients;
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
        private readonly IBraindumpRestClient restClient;

        /// <summary>
        /// This abstraction in form of a service layer is intended 
        /// to manage an offline data store in the future.
        /// </summary>
        public BraindumService(
            ILogger<BraindumService> logger,
            IBraindumpRestClient restClient)
        {
            this.logger = logger;
            this.restClient = restClient;
        }

        public async Task AddNeuron(string information, List<string> groups)
        {
            var neuron = new Neuron(information)
            {
                Id = Guid.NewGuid(),
                Groups = groups
            };            

            await restClient.AddNeuron(neuron);
        }

        public async Task<List<Neuron>> GetAll()
        {
            return await restClient.GetAllNeuronsAsync();
        }

        public async Task<List<Neuron>> GetNeuronsByGroup(string group)
        {
            return await restClient.GetNeuronsByGroupAsync(group);
        }
    }
}
