using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nudelsieb.Domain
{
    public interface INeuronRepository
    {
        Task<List<Neuron>> GetAllAsync();

        Task<List<Neuron>> GetByGroupAsync(string group);

        Task AddAsync(Neuron neuron);
        Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until);
        Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until, ReminderState state);
    }
}
