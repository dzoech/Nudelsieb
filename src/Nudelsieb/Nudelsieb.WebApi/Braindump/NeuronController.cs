using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nudelsieb.Application.Persistence;
using Nudelsieb.Application.UseCases;
using Nudelsieb.Domain;

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
        private readonly ISetReminderUseCase setReminderUseCase;

        public NeuronController(
            ILogger<NeuronController> logger,
            INeuronRepository neuronRepository,
            ISetReminderUseCase setReminderUseCase)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.neuronRepository = neuronRepository ?? throw new ArgumentNullException(nameof(neuronRepository));
            this.setReminderUseCase = setReminderUseCase ?? throw new ArgumentNullException(nameof(setReminderUseCase));
        }

        [HttpGet]
        public async Task<IEnumerable<NeuronDto>> GetAsync()
        {
            var neurons = await this.neuronRepository.GetAllAsync();
            var dtos = neurons.Select(n => new NeuronDto(n));
            return dtos;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<int>))]
        public async Task<ActionResult<List<int>>> Add([FromBody] NeuronDto neuronDto)
        {
            this.logger.LogInformation($"POST {neuronDto.Information}");

            var neuron = new Neuron(neuronDto.Information)
            {
                Id = neuronDto.Id,
                Groups = neuronDto.Groups
            };

            var reminders = neuronDto.Reminders.ToArray();
            var success = neuron.SetReminders(reminders, out _, out List<DateTimeOffset> errors);

            await this.neuronRepository.AddAsync(neuron);

            var success = await setReminderUseCase.ExecuteAsync(neuron.Id, reminders);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return Ok(new { ErrorIndices = errors });
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
            var success = await setReminderUseCase.ExecuteAsync(id, remindAt);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
