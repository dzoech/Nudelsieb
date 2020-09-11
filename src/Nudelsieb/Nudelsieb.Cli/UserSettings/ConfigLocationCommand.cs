using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Nudelsieb.Cli.UserSettings
{
    [Command(Name = "location")]
    class ConfigLocationCommand : CommandBase
    {
        private readonly IConsole console;
        private readonly IUserSettingsService userSettingsService;

        public ConfigLocationCommand(IConsole console, IUserSettingsService userSettingsService)
        {
            this.console = console;
            this.userSettingsService = userSettingsService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            console.WriteLine(userSettingsService.Location);
            return await base.OnExecuteAsync(app);
        }
    }
}
