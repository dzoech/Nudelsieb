using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Nudelsieb.Cli.Options;
using Nudelsieb.Cli.Services;

namespace Nudelsieb.Cli
{
    class LoginCommand : CommandBase
    {
        [Argument(0)]
        public string? Message { get; set; }

        [Option]
        public string[]? Groups { get; set; }

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
