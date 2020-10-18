using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Parsers;
using Nudelsieb.Cli.Services;

namespace Nudelsieb.Cli.Commands
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
        private readonly IConsole console;
        private readonly IReminderParser reminderParser;
        private readonly IGroupParser groupParser;

        public AddCommand(
            IBraindumpService braindumpService,
            IConsole console,
            IReminderParser reminderParser,
            IGroupParser groupParser)
        {
            this.braindumpService = braindumpService;
            this.console = console;
            this.reminderParser = reminderParser;
            this.groupParser = groupParser;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var groupNames = new List<string>();
            var invalidGroups = new List<string>();

            foreach (var g in Groups)
            {
                if (groupParser.TryParse(g, out var groupName))
                {
                    groupNames.Add(groupName);
                }
                else
                {
                    invalidGroups.Add(g);
                }
            }

            if (invalidGroups.Any())
            {
                throw new ArgumentException($"Could not parse these {invalidGroups.Count} group(s): {string.Join(", ", invalidGroups)}.");
            }

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

            if (invalidReminders.Any())
            {
                console.WriteLine(
                    $"Could not parse these {invalidReminders.Count} reminder(s): {string.Join(", ", invalidReminders)}.");
            }

            // Message is not null because it is [Required]
            await this.braindumpService.AddNeuron(Message!, Groups.ToList(), reminderTimes);

            return await base.OnExecuteAsync(app);
        }
    }
}
