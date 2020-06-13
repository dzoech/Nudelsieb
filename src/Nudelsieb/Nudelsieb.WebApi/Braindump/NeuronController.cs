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

        public NeuronController(ILogger<NeuronController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<NeuronDto> Get()
        {
            return new List<NeuronDto>
            {
                new NeuronDto("An unbelievable idea that I would otherwise forget instantly...") 
                {
                    Groups = new List<string> { "projects", "programming" } 
                },
                new NeuronDto("Remind me to turn off the oven."),
                new NeuronDto("Keep in mind that the API is only a dummy yet.")
                {
                    Groups = new List<string> { "programming" }
                },
                new NeuronDto("Why is my kitchen burning down?")
            };
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
