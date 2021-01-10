using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Nudelsieb.Shared.Clients.Models;

namespace Nudelsieb.Shared.Clients.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> logger;
        private readonly IPublicClientApplication clientApplication;
        private readonly IOptions<AuthOptions> authOptions;

        public AuthenticationService(
            ILogger<AuthenticationService> logger,
            IPublicClientApplication clientApplication,
            IOptions<AuthOptions> authOptions)
        {
            this.logger = logger;
            this.clientApplication = clientApplication;
            this.authOptions = authOptions;
        }

        public async Task<(bool Success, JwtSecurityToken? AccessToken)> GetCachedAccessTokenAsync()
        {
            // get accounts from token cache
            var accounts = await this.clientApplication.GetAccountsAsync();

            if (accounts.Count() == 0)
            {
                return (Success: false, AccessToken: null);
            }

            try
            {
                AuthenticationResult result = await this.clientApplication
                    .AcquireTokenSilent(this.authOptions.Value.RequiredScopes, accounts.FirstOrDefault())
                    .ExecuteAsync();

                var accessToken = ExtractTokens(result).AccessToken;

                return (Success: true, AccessToken: accessToken);

            }
            catch (MsalUiRequiredException ex) when (ex.Message.StartsWith("AADB2C90080"))
            {
                // AADB2C90080: The provided grant has expired. Please re-authenticate and try again.
                var (_, accessToken) = await LoginAsync();
                return (true, accessToken);
            }
            catch (MsalUiRequiredException ex)
            {
                this.logger.LogInformation($"{nameof(MsalUiRequiredException)}: {ex}");
                return (Success: false, AccessToken: null);
            }
        }

        public async Task<(JwtSecurityToken IdToken, JwtSecurityToken AccessToken)> LoginAsync()
        {
            if (this.authOptions.Value.PolicySignUpSignIn is null)
            {
                throw new ArgumentNullException(nameof(authOptions.Value.PolicySignUpSignIn));
            }
                
            var accounts = await this.clientApplication.GetAccountsAsync();

            var result = await this.clientApplication
                .AcquireTokenInteractive(authOptions.Value.RequiredScopes)
                .WithAccount(GetAccountByPolicy(accounts, authOptions.Value.PolicySignUpSignIn))
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

        private IAccount GetAccountByPolicy(IEnumerable<IAccount> accounts, string policy)
        {
            foreach (var account in accounts)
            {
                string userIdentifier = account.HomeAccountId.ObjectId.Split('.')[0];

                if (userIdentifier.EndsWith(policy.ToLower()))
                {
                    return account;
                }
            }
            return null!; // todo non null
        }
    }
}
