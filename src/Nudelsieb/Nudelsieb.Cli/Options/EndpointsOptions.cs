using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Cli.Options
{
    class EndpointsOptions
    {
        public const string SectionName = "Endpoints";
        public EndpointValue? Braindump { get; set; }

        public class EndpointValue
        {
            public string? Value { get; set; }
        }
    }
}
