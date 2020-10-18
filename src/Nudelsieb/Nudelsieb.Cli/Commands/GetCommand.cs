using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Parsers;
using Nudelsieb.Cli.Services;

namespace Nudelsieb.Cli.Commands
{
    class GetCommand : CommandBase
    {
        private readonly IBraindumpService braindumpService;
        private readonly IConsole console;
        private readonly IGroupParser groupParser;

        [Option(Description = "The name of the group for which all neurons are listed.")]
        public string Group { get; set; } = string.Empty;

        public GetCommand(
            IBraindumpService braindumpService,
            IConsole console,
            IGroupParser groupParser)
        {
            this.braindumpService = braindumpService;
            this.console = console;
            this.groupParser = groupParser;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            List<Models.Neuron> neurons;

            if (string.IsNullOrEmpty(Group))
            {
                neurons = await this.braindumpService.GetAllAsync();
            }
            else
            {
                if (groupParser.TryParse(Group, out var groupName))
                {
                    neurons = await this.braindumpService.GetNeuronsByGroupAsync(groupName);
                }
                else
                {
                    throw new ArgumentException(groupParser.ErrorMessage, nameof(Group));
                }
            }

            foreach (var n in neurons)
            {
                console.WriteLine(n.Information);
                if (n.Groups.Count > 0)
                {
                    console.WriteLine($"  Groups: {string.Join(", ", n.Groups)}");
                }

                if (n.Reminders.Count > 0)
                {
                    // display the reminders in ascending order
                    n.Reminders.Sort();

                    var reminderTimeSpans = new string[n.Reminders.Count];

                    for (int i = 0; i < n.Reminders.Count; i++)
                    {
                        var timeSpan = n.Reminders[i] - DateTimeOffset.Now;
                        reminderTimeSpans[i] = FormatTimeSpan(timeSpan);
                    }

                    console.WriteLine($"  Reminders: {string.Join(", ", reminderTimeSpans)}");
                }
            }

            return await base.OnExecuteAsync(app);
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            var s = string.Empty;

            if (timeSpan <= TimeSpan.Zero)
            {
                s = "Overdue for ";
            }

            if (timeSpan.Days != 0)
            {
                s += timeSpan.ToString(@"%d'd '%h'h'");
            }
            else
            {
                s += timeSpan.ToString(@"%h'h '%m'm'");
            }

            return s;
        }
    }
}
