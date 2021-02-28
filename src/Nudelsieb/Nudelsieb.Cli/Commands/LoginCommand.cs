using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Nudelsieb.Shared.Clients.Authentication;

namespace Nudelsieb.Cli.Commands
{
    class LoginCommand : CommandBase
    {
        private readonly ILogger<LoginCommand> logger;
        private readonly IConsole console;
        private readonly IAuthenticationService authService;

        public LoginCommand(
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
                var (idToken, _) = await authService.LoginAsync();

                if (idToken is null)
                    throw new Exception("Could not retrieve login user and retrieve their id token.");

                var user = authService.GetUserFromIdToken(idToken);
                this.console.WriteLine($"Hello {user.GivenName}! You are logged in as '{user.Email ?? ""}'.");
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
