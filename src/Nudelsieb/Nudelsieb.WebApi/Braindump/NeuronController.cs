using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nudelsieb.Application.UseCases;

namespace Nudelsieb.WebApi.Braindump
{
    [Area("braindump")]
    [Authorize]
    [Route("[area]/[controller]")]
    [ApiController]
    public class NeuronController : ControllerBase
    {
        private readonly ILogger<NeuronController> logger;
        private readonly SetRemindersUseCase setRemindersUseCase;
        private readonly NeurogenesisUseCase neurogenesisUseCase;
        private readonly GetEverythingUseCase getEverythingUseCase;
        private readonly UserService userService;

        public NeuronController(
            ILogger<NeuronController> logger,
            SetRemindersUseCase setReminderUseCase,
            NeurogenesisUseCase neurogenesisUseCase,
            GetEverythingUseCase getEverythingUseCase,
            UserService userService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.setRemindersUseCase = setReminderUseCase ?? throw new ArgumentNullException(nameof(setReminderUseCase));
            this.neurogenesisUseCase = neurogenesisUseCase ?? throw new ArgumentNullException(nameof(neurogenesisUseCase));
            this.getEverythingUseCase = getEverythingUseCase ?? throw new ArgumentNullException(nameof(getEverythingUseCase));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
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
            var activeUserId = userService.GetActiveUserId();
            var neuron = await neurogenesisUseCase.ExecuteAsync(neuronDto.Information, neuronDto.Groups);

            (bool success, var faultyReminders) = await setRemindersUseCase.ExecuteAsync(
                neuron.Id, activeUserId, neuronDto.Reminders.ToArray());

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
            var activeUserId = userService.GetActiveUserId();
            (bool success, var faultyReminders) = await setRemindersUseCase.ExecuteAsync(id, activeUserId, remindAt);

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
