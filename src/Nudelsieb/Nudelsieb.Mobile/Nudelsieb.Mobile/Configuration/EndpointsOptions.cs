#nullable enable

namespace Nudelsieb.Mobile.Configuration
{
    public class EndpointsOptions
    {
        public const string SectionName = "Endpoints";
        public EndpointValue? Braindump { get; set; }
        public EndpointValue? Notifications { get; set; }

        public class EndpointValue
        {
            public string? Value { get; set; }
        }
    }
}
