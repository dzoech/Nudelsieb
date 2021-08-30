using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nudelsieb.Domain.Abstractions;

namespace Nudelsieb.Domain.Aggregates
{
    /// <summary>
    /// Query -> instant
    /// Update/Write -> commit via UoW
    /// </summary>
    public interface INeuronRepository : IRepository<Neuron>
    {
        Task<Neuron> GetByIdAsync(Guid id);

        Task<List<Neuron>> GetAllAsync();

        Task<List<Neuron>> GetByGroupAsync(string group);

        Neuron Add(Neuron neuron);

        Neuron Update(Neuron neuron);


        // ReminderRepository:
        //Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until);
        //Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until, ReminderState state);
        //Task AddRemindersAsync(List<Reminder> reminders);
    }
}
