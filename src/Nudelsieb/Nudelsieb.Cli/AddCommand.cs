using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Models;
using Nudelsieb.Cli.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace Nudelsieb.Cli
{
    class AddCommand : CommandBase
    {
        [Argument(0)]
        [Required]
        public string? Message { get; set; }

        [Option]
        public string[]? Groups { get; set; }

        private readonly IBraindumpService braindumpService;

        public AddCommand(IBraindumpService braindumpService)
        {
            this.braindumpService = braindumpService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var neuron = new Neuron(Message!) // Message is not null because it is [Required]
            {
                Id = Guid.NewGuid()
            };

            await this.braindumpService.Add(neuron);

            return await base.OnExecuteAsync(app);
        }
    }
}
