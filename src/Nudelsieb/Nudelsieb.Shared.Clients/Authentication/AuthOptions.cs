using System;
using System.Collections.Generic;
using System.Linq;

namespace Nudelsieb.Shared.Clients.Authentication
{
    public class AuthOptions
    {
        public const string SectionName = "Auth";
        private string[] requiredScopes = Array.Empty<string>();
        public List<string> TestProp { get; set; } = new List<string>();
        public string? ClientId { get; set; }

        public string? TenantName { get; set; }

        public string? PolicySignUpSignIn { get; set; }

        public string? PolicyPasswortReset { get; set; }

        public string AadTenant => $"{TenantName}.onmicrosoft.com";

        public string? RedirectUri { get; set; }

        public string[] RequiredScopes
        {
            get => this.requiredScopes.Select(s => ReplacePlaceholder(s)).ToArray();
            set => this.requiredScopes = value;
        }

        public string? ClientSecret { get; set; }

        public string AuthoritySignUpSignin => $"https://{TenantName}.b2clogin.com/tfp/{AadTenant}/{PolicySignUpSignIn}";

        public string AuthorityPasswordReset => $"https://{TenantName}.b2clogin.com/tfp/{AadTenant}/{PolicyPasswortReset}";

        public CacheOptions Cache { get; set; } = new CacheOptions();
        private string ReplacePlaceholder(string scope)
        {
            return scope.Replace("{AadTenantUri}", $"https://{AadTenant}");
        }
    }
}
