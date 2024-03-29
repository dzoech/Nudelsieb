﻿using System.Collections.Generic;
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
    public class GroupController : ControllerBase
    {
        private readonly ILogger<GroupController> logger;
        private readonly INeuronRepository neuronRepository;

        public GroupController(ILogger<GroupController> logger, INeuronRepository neuronRepository)
        {
            this.logger = logger;
            this.neuronRepository = neuronRepository;
        }

        [HttpGet("{name}/neuron")]
        public async Task<IEnumerable<NeuronDto>> GetNeuronsForGroupAsync(string name)
        {
            var neurons = await neuronRepository.GetByGroupAsync(name);
            var dtos = neurons.Select(n => new NeuronDto
            {
                Id = n.Id,
                Information = n.Information,
                Groups = n.Groups.Select(g => g.Name).ToList()

                // TODO get reminders
            });
            return dtos;
        }
    }
}
