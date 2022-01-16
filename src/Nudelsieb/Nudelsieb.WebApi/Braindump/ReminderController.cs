using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nudelsieb.Application.UseCases;

namespace Nudelsieb.WebApi.Braindump
{
    [Area("braindump")]
    [Authorize]
    [Route("[area]/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly GetRemindersUseCase getRemindersUseCase;

        public ReminderController(GetRemindersUseCase getRemindersUseCase)
        {
            this.getRemindersUseCase = getRemindersUseCase ?? throw new ArgumentNullException(nameof(getRemindersUseCase));
        }

        [HttpGet]
        public async Task<IEnumerable<ReminderDto>> GetRemindersAsync([FromQuery] DateTimeOffset until)
        {
            return await getRemindersUseCase.ExecuteAsync(until);
        }
    }
}
