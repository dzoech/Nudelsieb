using System;
using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.Hosting.CommandLine;

namespace Nudelsieb.Cli
{
    internal class GlobalExceptionHandler : IUnhandledExceptionHandler
    {
        private readonly IConsole console;

        public GlobalExceptionHandler(IConsole console)
        {
            this.console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public void HandleException(Exception ex)
        {
            switch (ex)
            {
                case UnrecognizedCommandParsingException _:
                {
                    console.Error.WriteLine($"Error: {ex.Message}");
                    break;
                }

                default:
                {
                    console.Error.WriteLine($"Error ({ex.GetType()}): {ex.Message}");
                    console.Error.WriteLine($"Stack trace: {ex.StackTrace}");
                    break;
                }
            }
        }
    }
}
