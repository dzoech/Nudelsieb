using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nudelsieb.Domain.Aggregates;

namespace Nudelsieb.Application.UseCases
{
    public class GetEverythingUseCase
    {
        private readonly INeuronRepository neuronRepository;
        private readonly IReminderRepository reminderRepository;

        public GetEverythingUseCase(INeuronRepository neuronRepository, IReminderRepository reminderRepository)
        {
            this.neuronRepository = neuronRepository ?? throw new ArgumentNullException(nameof(neuronRepository));
            this.reminderRepository = reminderRepository ?? throw new ArgumentNullException(nameof(reminderRepository));
        }

        public async Task<EverythingDto> ExecuteAsync()
        {
            {
                var neurons = await this.neuronRepository.GetAllAsync();
                var reminders = await this.reminderRepository.GetRemindersAsync(DateTimeOffset.MaxValue);

                return new EverythingDto
                {
                    Neurons = neurons,
                    Reminders = reminders
                };
            }
        }
    }
}
