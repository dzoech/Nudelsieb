using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nudelsieb.Domain.Abstractions;

namespace Nudelsieb.Domain.Aggregates
{
    public interface IReminderRepository : IRepository<Reminder>
    {
        Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until);

        Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until, ReminderState state);

        Task AddAsync(Reminder reminder);
    }
}
