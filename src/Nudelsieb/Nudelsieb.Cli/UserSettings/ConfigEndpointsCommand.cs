using System;
using System.Collections.Generic;
using System.Text;
using McMaster.Extensions.CommandLineUtils;

namespace Nudelsieb.Cli.UserSettings
{
    [Command(Name = "endpoints")]
    [Subcommand(
        typeof(ConfigEndpointsSwitchCommand),
        typeof(ConfigEndpointsSetCommand))]
    class ConfigEndpointsCommand : CommandBase
    {
    }
}
