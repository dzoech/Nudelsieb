using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Parsers;
using Nudelsieb.Cli.Services;

namespace Nudelsieb.Cli.Commands.Reminder
{
    [Subcommand(typeof(ReminderGetCommand))]
    partial class ReminderCommand : CommandBase
    {
        [Command("get")]
        class ReminderGetCommand : CommandBase
        {
            private readonly IBraindumpService braindumpService;
            private readonly IConsole console;
            private readonly IReminderParser reminderParser;

            public string Until { get; set; } = "7d";

            public ReminderGetCommand(
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

                foreach (var r in reminders)
                {
                    var formatedReminder = FormatTimeSpan(r.At - DateTimeOffset.Now);
                    console.Write($"{formatedReminder} | {r.NeuronInformation}");

                    if (r.NeuronGroups.Length > 0)
                    {
                        console.Write($" | Groups: #{string.Join(", #", r.NeuronGroups)}");
                    }

                    console.WriteLine();
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
}
