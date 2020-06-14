using McMaster.Extensions.CommandLineUtils;

namespace Nudelsieb.Cli
{
    [Subcommand(
        typeof(GetAllCommand))]
    class GetCommand : CommandBase
    {
    }
}
