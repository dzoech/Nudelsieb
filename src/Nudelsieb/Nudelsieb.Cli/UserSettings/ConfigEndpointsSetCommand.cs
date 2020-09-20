using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Nudelsieb.Cli.UserSettings
{

    [Command(
        Name = "set", 
        Description = "Sets the braindum endpoint to a new URL.")]
    class ConfigEndpointsSetCommand : CommandBase
    {
        private readonly IConsole console;
        private readonly IUserSettingsService userSettingsService;

        [Required]
        [Argument(0)]
        [Url]
        public string? EndpointUrl { get; set; }

        public ConfigEndpointsSetCommand(IConsole console, IUserSettingsService userSettingsService)
        {
            this.console = console;
            this.userSettingsService = userSettingsService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var settings = await this.userSettingsService.ReadAsync();
            this.userSettingsService.SetEndpoint(settings.Endpoints.Braindump, EndpointUrl!);
            await this.userSettingsService.Write(settings);

            // Just printing the settings in a quick and dirty way
            console.WriteLine($"Changed {nameof(settings.Endpoints.Braindump)} endpoint to {settings.Endpoints.Braindump.Value}");

            return await base.OnExecuteAsync(app);
        }
    }
}
