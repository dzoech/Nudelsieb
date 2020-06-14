using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nudelsieb.Domain;

namespace Nudelsieb.WebApi.Braindump
{
    [Area("braindump")]
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
            var neurons = await neuronRepository.GetAllAsync();

            var dtos = neurons.Select(n => 
                new NeuronDto
                {
                    Information = n.Information,
                    Id = n.Id,
                    Groups = n.Groups
                });

            return dtos;
        }


        [HttpPost]
        public void Post([FromBody] NeuronDto neuronDto)
        {
            this.logger.LogInformation($"POST {neuronDto.Information}");
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
