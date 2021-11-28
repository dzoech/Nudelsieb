using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public ReminderController(ILogger<ReminderController> logger, IReminderRepository neuronRepository)
        {
            this.logger = logger;
            this.reminderRepository = neuronRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ReminderDto>> GetRemindersAsync([FromQuery] DateTimeOffset until)
        {
            // TODO #DDD move logic into Use Case
            var reminders = await reminderRepository.GetRemindersAsync(until);
            var dtos = reminders.Select(r => new ReminderDto(r)
            {
                //NeuronInformation = reminder.Subject.Information,
                //NeuronGroups = reminder.Subject.Groups
            });
            return dtos;
        }
    }
}
