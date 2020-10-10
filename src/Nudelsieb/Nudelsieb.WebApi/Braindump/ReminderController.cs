﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nudelsieb.Domain;

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
        public async Task<IEnumerable<NeuronDto>> GetRemindersAsync([FromQuery] DateTimeOffset until)
        {
            var neurons = await neuronRepository.GetRemindersAsync(until);

            var dtos = neurons.Select(n =>
                new NeuronDto
                {
                    Information = n.Information,
                    Id = n.Id,
                    Groups = n.Groups
                });

            return dtos;
        }
    }
}
