using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nudelsieb.Mobile.Services
{
    public interface IDeviceService
    {
        Task<string> GetPnsHandleAsync();
        Task SavePnsHandleAsync(string handle);
        void ClearPnsHandle();
        string GetDeviceId();
    }
}
