using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nudelsieb.Application.UseCases;
using Nudelsieb.Domain.Aggregates;

namespace Nudelsieb.WebApi.Braindump
{
    [Area("braindump")]
    [Authorize]
    [Route("[area]/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly ILogger<ReminderController> logger;
        private readonly IReminderRepository reminderRepository;
        private readonly GetRemindersUseCase getRemindersUseCase;

        public ReminderController(ILogger<ReminderController> logger, IReminderRepository neuronRepository)
        {
            this.logger = logger;
            this.reminderRepository = neuronRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ReminderDto>> GetRemindersAsync([FromQuery] DateTimeOffset until)
        {
            return await getRemindersUseCase.ExecuteAsync(until);
        }
    }
}
