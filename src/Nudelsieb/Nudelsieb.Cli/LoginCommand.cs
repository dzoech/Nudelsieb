using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.JsonWebTokens;
using Nudelsieb.Cli.Models;
using Nudelsieb.Cli.Options;
using Nudelsieb.Cli.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nudelsieb.Cli
{
    class LoginCommand : CommandBase
    {
        [Argument(0)]
        public string? Message { get; set; }

        [Option]
        public string[]? Groups { get; set; }

        private readonly IConsole console;
        private readonly IPublicClientApplication clientApplication;
        private readonly IConfiguration config;

        public LoginCommand(IConsole console, IPublicClientApplication clientApplication, IConfiguration config)
        {
            this.console = console;
            this.clientApplication = clientApplication;
            this.config = config;
        }

        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var authOptions = new AuthOptions();
            this.config.GetSection(AuthOptions.SectionName).Bind(authOptions);

            if (authOptions.PolicySignUpSignIn is null)
            {
                throw new ArgumentNullException(nameof(authOptions.PolicySignUpSignIn));
            }

            try
            {
                // call aad b2c
                var accounts = await this.clientApplication.GetAccountsAsync();

                var result = await this.clientApplication
                    .AcquireTokenInteractive(authOptions.RequiredScopes)
                    .WithAccount(GetAccountByPolicy(accounts, authOptions.PolicySignUpSignIn))
                    .ExecuteAsync();

                var idToken = new JwtSecurityTokenHandler().ReadJwtToken(result.IdToken);
                var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);

                var user = new
                {
                    GivenName = idToken.Claims.Single(c => c.Type == "given_name").Value,
                    Email = idToken.Claims.SingleOrDefault(c => c.Type == "emails")?.Value
                };

                this.console.WriteLine($"Hello {user.GivenName}! You are logged in as '{user.Email ?? ""}'.");
            }
            catch (Exception ex)
            {
                console.WriteLine(ex);
            }

            return await base.OnExecuteAsync(app);
        }

        private IAccount GetAccountByPolicy(IEnumerable<IAccount> accounts, string policy)
        {
            foreach (var account in accounts)
            {
                string userIdentifier = account.HomeAccountId.ObjectId.Split('.')[0];

                if (userIdentifier.EndsWith(policy.ToLower()))
                    return account;
            }
            return null!; // todo non null
        }
    }
}
