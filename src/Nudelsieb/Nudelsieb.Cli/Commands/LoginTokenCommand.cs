using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Nudelsieb.Shared.Clients.Authentication;

namespace Nudelsieb.Cli.Commands
{
    [Command(names: new[] { "token", "authtoken" })]
    class LoginTokenCommand : CommandBase
    {
        private readonly ILogger<LoginCommand> logger;
        private readonly IConsole console;
        private readonly IAuthenticationService authService;

        public LoginTokenCommand(
            ILogger<LoginCommand> logger,
            IConsole console,
            IAuthenticationService authService)
        {
            this.logger = logger;
            this.console = console;
            this.authService = authService;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            try
            {
                var (success, accessToken) = await authService.GetCachedAccessTokenAsync();

                if (success)
                {
                    this.console.WriteLine(accessToken!.RawData);
                } else
                {
                    this.console.Error.WriteLine("Please login before requesting the cached access token.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while authenticating user.");
                console.Error.WriteLine("Error during authentication. Please make sure you are connected to the internet, and try again.");
            }

            return await base.OnExecuteAsync(app);
        }

    }
}
