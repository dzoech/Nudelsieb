using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Cli.Options
{
    class AuthOptions
    {
        public string ClientId { get; set; }

        public string TenantName { get; set; }

        public string PolicySignUpSignIn { get; set; }

        public string AadTenant => $"{TenantName}.onmicrosoft.com";

        public string B2cAuthority => $"https://{TenantName}.b2clogin.com/tfp/{AadTenant}/{PolicySignUpSignIn}";
    }
}