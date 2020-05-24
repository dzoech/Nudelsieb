using McMaster.Extensions.CommandLineUtils;
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

        protected override int OnExecute(CommandLineApplication app)
        {
            Console.WriteLine($"Adding {Message} with {Groups.Length} groups: '{string.Join(", ", Groups)}'");

            return base.OnExecute(app);
        }
    }
}
