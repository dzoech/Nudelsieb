using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nudelsieb.Cli.Models;
using Refit;

namespace Nudelsieb.Cli.RestClients
{
    [Headers("User-Agent: Nudelsieb.Cli", "Authorization: Bearer")]
    interface IBraindumpRestClient
    {
        [Get("/Neuron/{id}")]
        Task<List<Neuron>> GetNeuronAsync(Guid id);

        [Get("/Neuron")]
        Task<List<Neuron>> GetAllNeuronsAsync();

        [Get("/Group/{groupName}/neuron")]
        Task<List<Neuron>> GetNeuronsByGroupAsync(string groupName);

        [Post("/Neuron")]
        Task AddNeuron(Neuron neuron);
    }
}
