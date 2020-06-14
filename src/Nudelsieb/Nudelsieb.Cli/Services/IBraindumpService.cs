using Nudelsieb.Cli.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nudelsieb.Cli.Services
{
    interface IBraindumpService
    {
        Task AddNeuron(string information, List<string> groups);

        Task<List<Neuron>> GetAll();
    }
}
