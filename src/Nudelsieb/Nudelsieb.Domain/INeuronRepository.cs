using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nudelsieb.Domain
{
    public interface INeuronRepository
    {
        Task<List<Neuron>> GetAllAsync();

        Task AddAsync(Neuron neuron);
    }
}
