using System;
using System.Threading.Tasks;

namespace Nudelsieb.Application.UseCases
{
    public interface ISetReminderUseCase
    {
        Task<bool> ExecuteAsync(Guid neuronId, params DateTimeOffset[] remindAt);
    }
}
