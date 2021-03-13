using System;
using System.Threading.Tasks;
using Nudelsieb.Application.Persistence;
using Nudelsieb.Domain;

namespace Nudelsieb.Application
{
    public class SetReminderUseCase
    {
        private readonly INeuronRepository neuronRepository;

        public SetReminderUseCase(INeuronRepository neuronRepository)
        {
            this.neuronRepository = neuronRepository ?? throw new ArgumentNullException(nameof(neuronRepository));
        }

        public async Task Execute(Neuron neuron, Reminder reminder)
        {

        }
    }
}
