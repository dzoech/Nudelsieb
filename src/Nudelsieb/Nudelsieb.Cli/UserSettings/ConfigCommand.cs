using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Nudelsieb.Cli.UserSettings
{

    [Subcommand(
        typeof(ConfigLocationCommand))]
    class ConfigCommand : CommandBase
    {
        private readonly IConsole console;
        private readonly IUserSettingsService userSettingsService;

        public ConfigCommand(IConsole console, IUserSettingsService userSettingsService)
        {
            this.console = console;
            this.userSettingsService = userSettingsService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var settings = await this.userSettingsService.Read();

            // Just printing the settings in a quick and dirty way
            console.WriteLine($"{nameof(settings.Endpoints.Braindump)}: {settings.Endpoints.Braindump}");
            console.WriteLine($"{nameof(settings.ConvertHashtagToGroup)}: {settings.ConvertHashtagToGroup}");

            return await base.OnExecuteAsync(app);
        }
    }
}
