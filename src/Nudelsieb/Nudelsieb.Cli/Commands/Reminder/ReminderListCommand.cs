using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Models;
using Nudelsieb.Cli.Parsers;
using Nudelsieb.Cli.Services;
using Nudelsieb.Cli.Utils;
using static Nudelsieb.Cli.Utils.CommandLineUtilsConsoleExtensions;

namespace Nudelsieb.Cli.Commands.Reminder
{
    [Command(names: new[] { "reminder", "reminders" })]
    [Subcommand(typeof(ReminderListCommand))]
    partial class ReminderCommand : CommandBase
    {
        [Command(names: new[] { "list", "get" })]
        class ReminderListCommand : CommandBase
        {
            private readonly IBraindumpService braindumpService;
            private readonly IConsole console;
            private readonly IReminderParser reminderParser;

            public string Until { get; set; } = "7d";

            public ReminderListCommand(
                IBraindumpService braindumpService,
                IConsole console,
                IReminderParser reminderParser)
            {
                this.braindumpService = braindumpService;
                this.console = console;
                this.reminderParser = reminderParser;
            }

            protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
            {
                List<Models.Reminder> reminders;

                if (reminderParser.TryParse(Until, out var timeSpan))
                {
                    var until = DateTimeOffset.Now + timeSpan;
                    console.WriteLine($"Showing reminders until {until}.");
                    reminders = await braindumpService.GetRemindersAsync(until);
                }
                else
                {
                    throw new ArgumentException("Could not parse reminder", nameof(Until));
                }

                console.WriteTable(
                    reminders,
                    r => new
                    {
                        r.Id,
                        Neuron = r.NeuronInformation,
                        Groups = $"{string.Join(", ", r.NeuronGroups)}",
                        DueIn = FormatTimeSpan(r.At - DateTimeOffset.Now)
                    },
                    r => r.IsOverdue ? Highlighting.Emphasize : Highlighting.None);

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
}
