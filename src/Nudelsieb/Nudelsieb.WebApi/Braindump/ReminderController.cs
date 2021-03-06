﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nudelsieb.Application.Persistence;

namespace Nudelsieb.WebApi.Braindump
{
    [Area("braindump")]
    [Authorize]
    [Route("[area]/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly ILogger<ReminderController> logger;
        private readonly INeuronRepository neuronRepository;

        public ReminderController(ILogger<ReminderController> logger, INeuronRepository neuronRepository)
        {
            this.logger = logger;
            this.neuronRepository = neuronRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ReminderDto>> GetRemindersAsync([FromQuery] DateTimeOffset until)
        {
            var reminders = await neuronRepository.GetRemindersAsync(until);
            var dtos = reminders.Select(r => new ReminderDto(r));
            return dtos;
        }
    }
}
