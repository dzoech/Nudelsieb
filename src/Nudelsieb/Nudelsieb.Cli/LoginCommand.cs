using McMaster.Extensions.CommandLineUtils;
using Microsoft.Identity.Client;
using Nudelsieb.Cli.Models;
using Nudelsieb.Cli.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Nudelsieb.Cli
{
    class LoginCommand : CommandBase
    {
        [Argument(0)]
        public string Message { get; set; }

        [Option]
        public string[] Groups { get; set; }

        private readonly IConsole console;
        private readonly IPublicClientApplication clientApplication;

        public LoginCommand(IConsole console, IPublicClientApplication clientApplication)
        {
            this.console = console;
            this.clientApplication = clientApplication;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            try
            {
                // call aad b2c
                var scope = new List<string> { "profile", "openid" };
                var res = await this.clientApplication
                    .AcquireTokenInteractive(scope)
                    .ExecuteAsync();
                
                this.console.WriteLine("Success!");
            }
            catch (Exception ex)
            {
                console.WriteLine(ex);
            }
            return await base.OnExecuteAsync(app);
        }
    }
}
