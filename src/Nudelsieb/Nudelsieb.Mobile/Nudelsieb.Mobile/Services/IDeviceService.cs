using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nudelsieb.Mobile.Services
{
    public interface IDeviceService
    {
        Task<string> GetHandleAsync();
        Task SavePnsHandle(string handle);
        void ClearPnsHandle();
        string GetDeviceId();
    }
}
