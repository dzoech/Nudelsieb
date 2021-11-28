using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nudelsieb.Domain.Abstractions;

namespace Nudelsieb.Domain.Aggregates
{
    /// <summary>
    /// Query: instant Update
    ///
    /// Write: commit via UoW
    /// </summary>
    public interface INeuronRepository : IRepository<Neuron>
    {
        Task<Neuron> GetByIdAsync(Guid id);

        Task<List<Neuron>> GetAllAsync();

        Task<List<Neuron>> GetByGroupAsync(string group);

        Task<Neuron> AddAsync(Neuron neuron);

        Task<Neuron> UpdateAsync(Neuron neuron);
    }
}
