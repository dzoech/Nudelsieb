using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Nudelsieb.Cli.Models;
using Nudelsieb.Cli.Options;

namespace Nudelsieb.Cli.Services
{
    class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> logger;
        private readonly IPublicClientApplication clientApplication;
        private readonly IConfiguration config;

        public AuthenticationService(
            ILogger<AuthenticationService> logger,
            IPublicClientApplication clientApplication,
            IConfiguration config)
        {
            this.logger = logger;
            this.clientApplication = clientApplication;
            this.config = config;
        }

        public async Task<(bool Success, JwtSecurityToken? AccessToken)> GetCachedAccessTokenAsync()
        {
            AuthOptions authOptions = GetAuthOptionsFromConfig(this.config);

            // get accounts from token cache
            var accounts = await this.clientApplication.GetAccountsAsync();

            if (accounts.Count() == 0)
            {
                return (Success: false, AccessToken: null);
            }

            try
            {
                AuthenticationResult result = await this.clientApplication
                    .AcquireTokenSilent(authOptions.RequiredScopes, accounts.FirstOrDefault())
                    .ExecuteAsync();

                var accessToken = ExtractTokens(result).AccessToken;

                return (Success: false, AccessToken: accessToken);

            }
            catch (MsalUiRequiredException ex)
            {
                this.logger.LogInformation($"{nameof(MsalUiRequiredException)}: {ex}");
                return (Success: false, AccessToken: null);
            }
        }

        public async Task<(JwtSecurityToken IdToken, JwtSecurityToken AccessToken)> LoginAsync()
        {
            var authOptions = GetAuthOptionsFromConfig(this.config);

            if (authOptions.PolicySignUpSignIn is null)
            {
                throw new ArgumentNullException(nameof(authOptions.PolicySignUpSignIn));
            }
                
            var accounts = await this.clientApplication.GetAccountsAsync();

            var result = await this.clientApplication
                .AcquireTokenInteractive(authOptions.RequiredScopes)
                .WithAccount(GetAccountByPolicy(accounts, authOptions.PolicySignUpSignIn))
                .ExecuteAsync();

            var token = ExtractTokens(result);

            return token;
        }

        public User GetUserFromIdToken(JwtSecurityToken idToken)
        {
            var givenName = idToken.Claims.Single(c => c.Type == "given_name").Value;
            var email = idToken.Claims.SingleOrDefault(c => c.Type == "emails")?.Value;

            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return new User(givenName, email);
        }

        private (JwtSecurityToken IdToken, JwtSecurityToken AccessToken) ExtractTokens(
            AuthenticationResult result)
        {
            // todo - remove after testing
            var tokenHandler = new JwtSecurityTokenHandler();
            var idToken = tokenHandler.ReadJwtToken(result.IdToken);
            var accessToken = tokenHandler.ReadJwtToken(result.AccessToken);

            return (idToken, accessToken);
        }

        private AuthOptions GetAuthOptionsFromConfig(IConfiguration config)
        {
            // Todo evaluate usage of IOptions
            var authOptions = new AuthOptions();
            config.GetSection(AuthOptions.SectionName).Bind(authOptions);
            return authOptions;
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
