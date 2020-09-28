using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Identity.Client;

namespace Nudelsieb.Cli.Options
{
    class AuthOptions
    {
        public const string SectionName = "Auth";

        public string? ClientId { get; set; }

        public string? TenantName { get; set; }

        public string? PolicySignUpSignIn { get; set; }

        public string AadTenant => $"{TenantName}.onmicrosoft.com";

        public string B2cAuthority => $"https://{TenantName}.b2clogin.com/tfp/{AadTenant}/{PolicySignUpSignIn}";

        public string? RedirectUri { get; set; }

        public List<string> RequiredScopes
        {
            get => requiredScopes.Select(s => ReplacePlaceholder(s)).ToList();
            set => requiredScopes = value;
        }

        public string? ClientSecret { get; set; }

        public CacheOptions Cache { get; set; } = new CacheOptions();

        public class CacheOptions
        {
            public string FileName { get; set; } = "cache";

            public string Directory { get; set; } = string.Empty;
        }

        private string ReplacePlaceholder(string scope)
        {
            return scope.Replace("{AadTenantUri}", $"https://{AadTenant}");
        }

        private List<string> requiredScopes = new List<string>();
    }
}