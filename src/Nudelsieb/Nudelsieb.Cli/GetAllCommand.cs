using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Nudelsieb.Cli.Services;

namespace Nudelsieb.Cli
{
    [Command(Name = "all")]
    class GetAllCommand : CommandBase
    {
        private readonly IConsole console;
        private readonly IBraindumpService braindumpService;

        public GetAllCommand(IBraindumpService braindumpService, IConsole console)
        {
            this.console = console;
            this.braindumpService = braindumpService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var neurons = await this.braindumpService.GetAll();

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
