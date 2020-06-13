using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nudelsieb.Cli.Models;
using Refit;

namespace Nudelsieb.Cli.RestClients
{
    interface IBraindumpRestClient
    {
        [Get("/Neuron/{id}")]
        Task<List<Neuron>> GetNeuron(Guid id);
    }
}
