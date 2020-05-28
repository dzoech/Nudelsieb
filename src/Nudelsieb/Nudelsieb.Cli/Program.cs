using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Nudelsieb.Cli.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Nudelsieb.Cli
{
    // See example: https://github.com/natemcmaster/CommandLineUtils/blob/master/docs/samples/subcommands/inheritance/Program.cs
    [HelpOption("-?|-h|--help")]
    abstract class CommandBase
    {
        protected virtual int OnExecute(CommandLineApplication app)
        {
            //Console.WriteLine("Result = nudelsieb " + ArgumentEscaper.EscapeAndConcatenate(args));
            return 0;
        }
    }

    [Command("nudelsieb")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(AddCommand))]
    class Program : CommandBase
    {
        public static async Task<int> Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(configBuilder =>
                {
                    configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                    configBuilder.AddJsonFile("appsettings.json");
                })
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.AddConfiguration(context.Configuration.GetSection("Logging"));
                    loggingBuilder.AddConsole(c => c.TimestampFormat = "HH:mm:ss"); // use .fff for milliseconds
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                        .AddSingleton<IBraindumpService, BraindumService>();
                });

            return await hostBuilder.RunCommandLineApplicationAsync<Program>(args);
        }

        /// <summary>
        /// This method is only executed if no subcommand can be matched to the provided args.
        /// </summary>
        protected override int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }

        private static string GetVersion()
            => typeof(Program)
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
    }
}
