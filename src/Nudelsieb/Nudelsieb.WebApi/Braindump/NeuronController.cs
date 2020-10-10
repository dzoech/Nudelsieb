using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public NeuronController(ILogger<NeuronController> logger, INeuronRepository neuronRepository)
        {
            this.logger = logger;
            this.neuronRepository = neuronRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<NeuronDto>> GetAsync()
        {
            var neurons = await this.neuronRepository.GetAllAsync();
            var dtos = neurons.Select(n => new NeuronDto(n));
            return dtos;
        }


        [HttpPost]
        public async Task Add([FromBody] NeuronDto neuronDto)
        {
            this.logger.LogInformation($"POST {neuronDto.Information}");

            var neuron = new Neuron(neuronDto.Information)
            {
                Id = neuronDto.Id,
                Groups = neuronDto.Groups
            };

            await this.neuronRepository.AddAsync(neuron);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] NeuronDto neuronDto)
        {
            this.logger.LogInformation($"PUT {id} -> {neuronDto.Information}");
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.logger.LogInformation($"DELETE {id}");
        }
    }
}
