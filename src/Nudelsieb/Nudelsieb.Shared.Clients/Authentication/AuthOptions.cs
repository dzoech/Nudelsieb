using System.Collections.Generic;
using System.Linq;

namespace Nudelsieb.Shared.Clients.Authentication
{
    public class AuthOptions
    {
        public List<string> TestProp { get; set; } = new List<string>();

        public const string SectionName = "Auth";

        public string? ClientId { get; set; }

        public string? TenantName { get; set; }

        public string? PolicySignUpSignIn { get; set; }

        public string? PolicyPasswortReset { get; set; }

        public string AadTenant => $"{TenantName}.onmicrosoft.com";

        public string? RedirectUri { get; set; }

        public List<string> RequiredScopes
        {
            get => requiredScopes.Select(s => ReplacePlaceholder(s)).ToList();
            set => requiredScopes = value;
        }

        public string? ClientSecret { get; set; }

        public string AuthoritySignUpSignin => $"https://{TenantName}.b2clogin.com/tfp/{AadTenant}/{PolicySignUpSignIn}";

        public string AuthorityPasswordReset => $"https://{TenantName}.b2clogin.com/tfp/{AadTenant}/{PolicyPasswortReset}";

        public CacheOptions Cache { get; set; } = new CacheOptions();

        private List<string> requiredScopes = new List<string>();

        private string ReplacePlaceholder(string scope)
        {
            return scope.Replace("{AadTenantUri}", $"https://{AadTenant}");
        }
    }
}