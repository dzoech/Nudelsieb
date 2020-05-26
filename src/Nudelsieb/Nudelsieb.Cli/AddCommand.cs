using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Models;
using Nudelsieb.Cli.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nudelsieb.Cli
{
    class AddCommand : CommandBase
    {
        [Argument(0)]
        [Required]
        public string Message { get; set; }

        [Option]
        public string[] Groups { get; set; }

        private readonly IBraindumpService braindumpService;

        public AddCommand(IBraindumpService braindumpService)
        {
            this.braindumpService = braindumpService;
        }

        protected override int OnExecute(CommandLineApplication app)
        {
            var neuron = new Neuron
            {
                Id = Guid.NewGuid(),
                Information = Message
            };

            this.braindumpService.Add(neuron);

            return base.OnExecute(app);
        }
    }
}
