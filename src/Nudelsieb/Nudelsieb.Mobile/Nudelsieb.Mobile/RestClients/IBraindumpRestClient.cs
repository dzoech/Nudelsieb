using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nudelsieb.Mobile.RestClients.Models;
using Refit;

namespace Nudelsieb.Mobile.RestClients
{
    [Headers("User-Agent: Nudelsieb.Mobile", "Authorization: Bearer")]
    public interface IBraindumpRestClient
    {
        [Get("/Reminder")]
        public Task<List<Reminder>> GetRemindersAsync(DateTimeOffset until);
    }
}
