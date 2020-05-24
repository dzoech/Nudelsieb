using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nudelsieb.Cli
{
    // See example: https://github.com/natemcmaster/CommandLineUtils/blob/master/docs/samples/subcommands/inheritance/Program.cs
    [HelpOption("-?|-h|--help")]
    abstract class CommandBase
    {
        protected virtual int OnExecute(CommandLineApplication app)
        {
            //Console.WriteLine("Result = nudelsieb " + ArgumentEscaper.EscapeAndConcatenate(args));
            app.ShowHelp();
            return 0;
        }
    }

    [Command("nudelsieb")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(AddCommand))]
    class Program : CommandBase
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        protected override int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }

        private static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
