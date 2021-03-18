using Microsoft.Azure.Cosmos;
using Nudelsieb.Application.Persistence;
using Nudelsieb.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nudelsieb.Persistence
{
    public class CosmosDbNeuronRepository
    {
        private readonly Container container;

        public CosmosDbNeuronRepository(Container container)
        {
            this.container = container;
        }

        public Task AddAsync(Neuron neuron)
        {
            throw new NotImplementedException();
        }

        public Task<List<Neuron>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Neuron>> GetByGroupAsync(string group)
        {
            throw new NotImplementedException();
        }

        public Task<Neuron> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until)
        {
            throw new NotImplementedException();
        }

        public Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until, ReminderState state)
        {
            throw new NotImplementedException();
        }

        public Task AddRemindersAsync(Neuron neuron)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRemindersAsync(Neuron neuron)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRemindersAsync(Guid neuronId, List<Reminder> reminders)
        {
            throw new NotImplementedException();
        }

        public Task AddRemindersAsync(List<Reminder> reminders)
        {
            throw new NotImplementedException();
        }
    }
}
