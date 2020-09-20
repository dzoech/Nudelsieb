using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Nudelsieb.Cli.UserSettings
{

    [Command(Name = "endpoints")]
    [Subcommand(typeof(ConfigEndpointsSwitchCommand))]
    class ConfigEndpointsCommand : CommandBase
    {
    }

    [Command(Name = "switch")]
    class ConfigEndpointsSwitchCommand : CommandBase
    {
        private readonly IConsole console;
        private readonly IUserSettingsService userSettingsService;

        public ConfigEndpointsSwitchCommand(IConsole console, IUserSettingsService userSettingsService)
        {
            this.console = console;
            this.userSettingsService = userSettingsService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var settings = await this.userSettingsService.ReadAsync();
            this.userSettingsService.SwitchEndpoint(settings.Endpoints.Braindump);
            await this.userSettingsService.Write(settings);

            // Just printing the settings in a quick and dirty way
            console.WriteLine($"Changed {nameof(settings.Endpoints.Braindump)} endpoint to {settings.Endpoints.Braindump.Value}");

            return await base.OnExecuteAsync(app);
        }
    }
}
