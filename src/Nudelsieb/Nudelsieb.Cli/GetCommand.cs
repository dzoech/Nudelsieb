﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Parsers;
using Nudelsieb.Cli.Services;

namespace Nudelsieb.Cli
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
                neurons = await this.braindumpService.GetAll();
            }
            else
            {
                if (groupParser.TryParse(Group, out var groupName))
                {
                    neurons = await this.braindumpService.GetNeuronsByGroup(groupName);
                }
                else
                {
                    throw new ArgumentException(groupParser.ErrorMessage, nameof(Group));
                }
            }

            foreach (var n in neurons)
            {
                console.WriteLine($"neuron: {n.Information}");
                if (n.Groups.Count > 0)
                {
                    console.WriteLine($"  groups: {string.Join(", ", n.Groups)}");
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

                    console.WriteLine($"  reminders: {string.Join(", ", reminderTimeSpans)}");
                }
            }

            return await base.OnExecuteAsync(app);
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.Days > 0)
            {
                return timeSpan.ToString(@"%d'd '%h'h'");
            }
            else
            {
                return timeSpan.ToString(@"%h'h '%m'm'");
            }
        }
    }
}
