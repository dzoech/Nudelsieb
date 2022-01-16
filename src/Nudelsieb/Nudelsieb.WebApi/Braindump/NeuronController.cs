using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class NeuronController : ControllerBase
    {
        private readonly ILogger<NeuronController> logger;
        private readonly INeuronRepository neuronRepository;
        private readonly IReminderRepository reminderRepository;
        private readonly SetRemindersUseCase setRemindersUseCase;
        private readonly NeurogenesisUseCase neurogenesisUseCase;
        private readonly GetEverythingUseCase getEverythingUseCase;

        public NeuronController(
            ILogger<NeuronController> logger,
            INeuronRepository neuronRepository,
            IReminderRepository reminderRepository,
            SetRemindersUseCase setReminderUseCase,
            NeurogenesisUseCase neurogenesisUseCase,
            GetEverythingUseCase getEverythingUseCase)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.neuronRepository = neuronRepository ?? throw new ArgumentNullException(nameof(neuronRepository));
            this.reminderRepository = reminderRepository ?? throw new ArgumentNullException(nameof(reminderRepository));
            this.setRemindersUseCase = setReminderUseCase ?? throw new ArgumentNullException(nameof(setReminderUseCase));
            this.neurogenesisUseCase = neurogenesisUseCase ?? throw new ArgumentNullException(nameof(neurogenesisUseCase));
            this.getEverythingUseCase = getEverythingUseCase ?? throw new ArgumentNullException(nameof(getEverythingUseCase));
        }

        [HttpGet]
        public async Task<IEnumerable<NeuronDto>> GetAsync()
        {
            var everything = await getEverythingUseCase.ExecuteAsync();

            var dtos = everything.Neurons.Select(n => new NeuronDto
            {
                Id = n.Id,
                Information = n.Information,
                Groups = n.Groups.Select(g => g.Name).ToList(),
                Reminders = everything.Reminders
                    .Where(r => r.NeuronReference == n.Id)
                    .Select(r => r.At)
                    .ToList()
            });
            return dtos;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<int>))]
        public async Task<ActionResult<List<int>>> Create([FromBody] NeuronDto neuronDto)
        {
            this.logger.LogInformation($"POST {neuronDto.Information}");
            var neuron = await neurogenesisUseCase.ExecuteAsync(neuronDto.Information, neuronDto.Groups);
            (bool success, var faultyReminders) = await setRemindersUseCase.ExecuteAsync(neuron.Id, neuronDto.Reminders.ToArray());

            if (success)
            {
                return NoContent();
            }
            else
            {
                return Ok(new { FaultyReminders = faultyReminders });
            }
        }

        /// <summary>
        /// Not implemented as of yet.
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] NeuronDto neuronDto)
        {
            this.logger.LogInformation($"PUT {id} -> {neuronDto?.Information}");
        }

        /// <summary>
        /// Not implemented as of yet.
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.logger.LogInformation($"DELETE {id}");
        }

        [HttpPost("{id}/reminder")]
        public async Task<ActionResult> AddReminderAsync(Guid id, [FromBody] DateTimeOffset remindAt)
        {
            (bool success, var faultyReminders) = await setRemindersUseCase.ExecuteAsync(id, remindAt);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new { FaultyReminders = faultyReminders });
            }
        }
    }
}
