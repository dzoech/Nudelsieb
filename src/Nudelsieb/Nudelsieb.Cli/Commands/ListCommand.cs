using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Parsers;
using Nudelsieb.Cli.Services;
using Nudelsieb.Cli.Utils;

namespace Nudelsieb.Cli.Commands
{
    [Command(names: new[] { "list", "get" })]
    internal class ListCommand : CommandBase
    {
        private readonly IBraindumpService braindumpService;
        private readonly IConsole console;
        private readonly IGroupParser groupParser;

        public ListCommand(
            IBraindumpService braindumpService,
            IConsole console,
            IGroupParser groupParser)
        {
            this.braindumpService = braindumpService;
            this.console = console;
            this.groupParser = groupParser;
        }

        [Option(Description = "The name of the group for which all neurons are listed.")]
        public string Group { get; set; } = string.Empty;

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            List<Models.Neuron> neurons;

            if (string.IsNullOrEmpty(Group))
            {
                neurons = await this.braindumpService.GetAllAsync();
            }
            else if (groupParser.TryParse(Group, out var groupName))
            {
                neurons = await this.braindumpService.GetNeuronsByGroupAsync(groupName);
            }
            else
            {
                throw new ArgumentException(groupParser.ErrorMessage, nameof(Group));
            }

            console.WriteTable(
                neurons,
                n => new
                {
                    n.Id,
                    n.Information,
                    Groups = string.Join(", ", n.Groups),
                    Reminders = string.Join(", ", n.Reminders.Select(r => FormatTimeSpan(r - DateTimeOffset.Now)))
                });

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
