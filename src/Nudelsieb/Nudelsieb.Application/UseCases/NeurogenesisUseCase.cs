using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nudelsieb.Domain.Aggregates;
using Nudelsieb.Domain.Aggregates.NeuronAggregate;

namespace Nudelsieb.Application.UseCases
{
    /// <summary>
    /// The process by which new neurons are formed in the brain.
    /// </summary>
    public class NeurogenesisUseCase
    {
        private readonly INeuronRepository neuronRepository;

        public NeurogenesisUseCase(INeuronRepository neuronRepository)
        {
            this.neuronRepository = neuronRepository ?? throw new ArgumentNullException(nameof(neuronRepository));
        }

        public async Task<Neuron> ExecuteAsync(string neuronInformation, IEnumerable<string> groupNames)
        {
            var neuron = new Neuron(neuronInformation);
            var groups = groupNames.Select(n => new Group(n));

            foreach (var group in groups)
            {
                neuron.AssignToGroup(group);
            }

            await neuronRepository.AddAsync(neuron);

            return neuron;
        }
    }
}
