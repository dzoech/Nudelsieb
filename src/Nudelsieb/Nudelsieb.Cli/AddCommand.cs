using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Parsers;
using Nudelsieb.Cli.Services;

namespace Nudelsieb.Cli
{
    class AddCommand : CommandBase
    {
        [Argument(0)]
        [Required]
        public string? Message { get; set; }

        [Option]
        public string[] Groups { get; set; } = Array.Empty<string>();

        [Option]
        public string[] Reminders { get; set; } = Array.Empty<string>();

        private readonly IBraindumpService braindumpService;
        private readonly IReminderParser reminderParser;

        public AddCommand(IBraindumpService braindumpService, IReminderParser reminderParser)
        {
            this.braindumpService = braindumpService;
            this.reminderParser = reminderParser;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var now = DateTimeOffset.Now;
            var reminderTimes = new List<DateTimeOffset>();
            var invalidReminders = new List<string>();

            foreach (var r in Reminders)
            {
                if (reminderParser.TryParse(r, out var time))
                {
                    reminderTimes.Add(now + time);
                }
                else
                {
                    invalidReminders.Add(r);
                }
            }

            // Message is not null because it is [Required]
            await this.braindumpService.AddNeuron(Message!, Groups.ToList(), reminderTimes);

            return await base.OnExecuteAsync(app);
        }
    }
}
