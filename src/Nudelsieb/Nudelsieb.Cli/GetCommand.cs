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
        private readonly IBraindumpService braindumpService;

        [Option]
        [Required]
        public string? Group { get; set; } //= string.Empty;

        public GetCommand(IBraindumpService braindumpService)
        {
            this.braindumpService = braindumpService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            // Message is not null because it is [Required]
            await this.braindumpService.GetNeuronsByGroup(Group!);

            // TODO handle http 404

            return await base.OnExecuteAsync(app);
        }
    }
}
