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
        private readonly NeurogenesisUseCase neurogenesisUseCase;
        private readonly SetRemindersUseCase setReminderUseCase;

        public NeuronController(
            ILogger<NeuronController> logger,
            INeuronRepository neuronRepository,
            SetRemindersUseCase setReminderUseCase)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.neuronRepository = neuronRepository ?? throw new ArgumentNullException(nameof(neuronRepository));
            this.setReminderUseCase = setReminderUseCase ?? throw new ArgumentNullException(nameof(setReminderUseCase));
        }

        [HttpGet]
        public async Task<IEnumerable<NeuronDto>> GetAsync()
        {
            // TODO #DDD utilize Use Case
            var neurons = await this.neuronRepository.GetAllAsync();
            var dtos = neurons.Select(n => new NeuronDto
            {
                Id = n.Id,
                Information = n.Information,
                Groups = n.Groups.Select(g => g.Name).ToList()
                // TODO set reminders
            });
            return dtos;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<int>))]
        public async Task<ActionResult<List<int>>> Create([FromBody] NeuronDto neuronDto)
        {
            this.logger.LogInformation($"POST {neuronDto.Information}");



            // TODO #DDD delegate this logic to a Use Case

            //var neuron = new Neuron(neuronDto.Information)
            //{
            //    Id = neuronDto.Id,
            //    Groups = neuronDto.Groups
            //};

            var reminders = neuronDto.Reminders.ToArray();
            //var success = neuron.SetReminders(reminders, out _, out List<DateTimeOffset> errors);
            bool success = false;

            //await this.neuronRepository.AddAsync(neuron);
            // TODO #DDD
            //var success = await setReminderUseCase.ExecuteAsync(neuron.Id, reminders);

            if (success)
            {
                return NoContent();
            }
            else
            {
                //return Ok(new { ErrorIndices = errors });
                return Ok("neuron added but no reminder has been set");
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
            (bool success, var faultyReminders) = await setReminderUseCase.ExecuteAsync(id, remindAt);

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
