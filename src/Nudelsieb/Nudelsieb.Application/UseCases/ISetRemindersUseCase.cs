using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nudelsieb.Application.UseCases
{
    public interface ISetRemindersUseCase
    {
        Task<(bool Success, IEnumerable<DateTimeOffset> FaultyReminders)> ExecuteAsync(Guid neuronId, params DateTimeOffset[] remindAt);
    }
}
