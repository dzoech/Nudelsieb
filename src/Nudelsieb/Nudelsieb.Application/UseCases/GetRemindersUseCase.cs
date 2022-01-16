using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nudelsieb.Domain.Aggregates;

namespace Nudelsieb.Application.UseCases
{
    public class GetRemindersUseCase
    {
        private readonly IReminderRepository reminderRepository;
        private readonly INeuronRepository neuronRepository;

        public GetRemindersUseCase(IReminderRepository reminderRepository, INeuronRepository neuronRepository)
        {
            this.reminderRepository = reminderRepository ?? throw new ArgumentNullException(nameof(reminderRepository));
            this.neuronRepository = neuronRepository ?? throw new ArgumentNullException(nameof(neuronRepository));
        }

        public async Task<IEnumerable<ReminderDto>> ExecuteAsync(DateTimeOffset until)
        {
            var reminders = await reminderRepository.GetRemindersAsync(until);
            var neurons = await neuronRepository.GetAllAsync();

            var dtos = reminders.Select(r =>
            {
                var referencedNeuron = neurons.Single(n => n.Id == r.NeuronReference);

                return new ReminderDto(r)
                {
                    NeuronInformation = referencedNeuron.Information,
                    NeuronGroups = referencedNeuron.Groups.Select(g => g.Name).ToList()
                };
            });

            return dtos;
        }
    }
}
