using Nudelsieb.Cli.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nudelsieb.Cli.Services
{
    interface IBraindumpService
    {
        Task AddNeuronAsync(string information, List<string> groups, List<DateTimeOffset> reminders);
        Task<List<Neuron>> GetAllAsync();
        Task<List<Neuron>> GetNeuronsByGroupAsync(string group);
        Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until);
    }
}
