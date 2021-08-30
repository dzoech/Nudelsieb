using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nudelsieb.Domain;
using Nudelsieb.Domain.Aggregates;

namespace Nudelsieb.Application.Persistence
{
    public interface INeuronRepository
    {
        Task<Neuron> GetByIdAsync(Guid id);

        Task<List<Neuron>> GetAllAsync();

        Task<List<Neuron>> GetByGroupAsync(string group);

        Task AddAsync(Neuron neuron);

        Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until);

        Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until, ReminderState state);

        Task AddRemindersAsync(List<Reminder> reminders);
    }
}
