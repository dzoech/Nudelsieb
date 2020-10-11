using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Services;

namespace Nudelsieb.Cli
{
    [Subcommand(
        typeof(GetAllCommand))]
    class GetCommand : CommandBase
    {
        private const string AlphanumericDashUnderscoreDigitRegex = @"^[\w\d-]+$";

        private readonly IBraindumpService braindumpService;
        private readonly IConsole console;

        [Option(Description = "The name of the group for which all neurons are listed.")]
        [RegularExpression(AlphanumericDashUnderscoreDigitRegex, ErrorMessage = "Group name must only consist of alphanumeric characters, digits, and dashes/hyphens (-).")]
        public string Group { get; set; } = string.Empty;

        public GetCommand(IBraindumpService braindumpService, IConsole console)
        {
            this.braindumpService = braindumpService;
            this.console = console;
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
                neurons = await this.braindumpService.GetNeuronsByGroup(Group);
            }

            foreach (var n in neurons)
            {
                console.WriteLine($"neuron: {n.Information}");
                if (n.Groups.Count > 0)
                {
                    console.WriteLine($"  groups: {string.Join(", ", n.Groups)}");
                }
            }

            return await base.OnExecuteAsync(app);
        }
    }
}
