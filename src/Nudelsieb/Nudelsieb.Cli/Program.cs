﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.Hosting.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using Nudelsieb.Cli.Commands;
using Nudelsieb.Cli.Commands.Reminder;
using Nudelsieb.Cli.Options;
using Nudelsieb.Cli.Parsers;
using Nudelsieb.Cli.RestClients;
using Nudelsieb.Cli.Services;
using Nudelsieb.Cli.UserSettings;
using Nudelsieb.Shared.Clients.Authentication;

namespace Nudelsieb.Cli
{
    // See example:
    // https://github.com/natemcmaster/CommandLineUtils/blob/main/docs/samples/subcommands/inheritance/Program.cs
    [HelpOption("-?|-h|--help")]
    public abstract class CommandBase
    {
        [Option("-d|--debug", Description = "Outputs debug information.")]
        public bool IsDebug { get; set; }

        protected virtual async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            //Console.WriteLine("Result = nudelsieb " + ArgumentEscaper.EscapeAndConcatenate(args));
            return await Task.FromResult(0);
        }
    }

    [Command("nudelsieb")]
    [VersionOptionFromMember("-v|--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(ListCommand),
        typeof(AddCommand),
        typeof(LoginCommand),
        typeof(ConfigCommand),
        typeof(ReminderCommand))]
    public class Program : CommandBase
    {
        private const Environment.SpecialFolder UserSettingsFolder = Environment.SpecialFolder.ApplicationData;

        private static readonly string UserSettingsLocation =
            Path.Combine(Environment.GetFolderPath(UserSettingsFolder), "nudelsieb");

        public static async Task<int> Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(configBuilder =>
                {
                    configBuilder.AddJsonFile("hostsettings.json", optional: true);
                })
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    // platform-independent way to get assembly path see
                    // https://github.com/dotnet/runtime/issues/13051#issuecomment-535654457
                    var executingAssembly = Process.GetCurrentProcess().MainModule.FileName;
                    var assemblyPath = Path.GetDirectoryName(executingAssembly);

                    Console.WriteLine($"Setting base path for configuration files to '{assemblyPath}'");

                    configBuilder.SetBasePath(assemblyPath);
                    configBuilder.AddJsonFile("appsettings.json");

                    // TODO sub dir and file name are defined here and in LocalUserSettingsService
                    // inject sub dir and file name settings into LocalUserSettingsService
                    var settingsFile = Path.Combine(UserSettingsLocation, "settings.json");

                    Console.WriteLine($"Loading optional configuration from '{settingsFile}'");
                    configBuilder.AddJsonFile(settingsFile, optional: true);

                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        configBuilder.AddUserSecrets<Program>();
                    }
                })
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.AddConfiguration(context.Configuration.GetSection("Logging"));
                    loggingBuilder.AddConsole(c => c.TimestampFormat = "HH:mm:ss"); // use .fff for milliseconds
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<AuthOptions>(o =>
                        context.Configuration.GetSection(
                            AuthOptions.SectionName).Bind(o));

                    services.Configure<EndpointsOptions>(o =>
                        context.Configuration.GetSection(
                            EndpointsOptions.SectionName).Bind(o));

                    var authOptions = new AuthOptions();
                    context.Configuration.GetSection(AuthOptions.SectionName).Bind(authOptions);

                    // using Microsoft.Identity.Client.Extensions.Msal as Cache
                    // https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/tree/master/src/Microsoft.Identity.Client.Extensions.Msal
                    var props = new StorageCreationPropertiesBuilder(
                            authOptions.Cache.FileName,
                            Path.Combine(UserSettingsLocation, authOptions.Cache.Directory))
                        .WithCacheChangedEvent(authOptions.ClientId)
                        .Build();

                    // composition root
                    services
                        .AddSingleton<IUnhandledExceptionHandler, GlobalExceptionHandler>()
                        .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                        .AddSingleton<IPublicClientApplication>(_ =>
                        {
                            var app = PublicClientApplicationBuilder
                              .Create(authOptions.ClientId)
                              .WithRedirectUri(authOptions.RedirectUri) // AAD B2C requires a port https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/System-Browser-on-.Net-Core#limitations
                              .WithB2CAuthority(authOptions.AuthoritySignUpSignin)
                              .Build();

                            var msalCacheHelper = MsalCacheHelper.CreateAsync(props).Result;
                            msalCacheHelper.RegisterCache(app.UserTokenCache);

                            return app;
                        })
                        .AddTransient<IBraindumpService, BraindumService>()
                        .AddTransient<IAuthenticationService, AuthenticationService>()
                        .AddTransient<IGroupParser, GroupParser>()
                        .AddTransient<IReminderParser>(_ => new ReminderParser(Thread.CurrentThread.CurrentCulture)
                        )
                        .AddRestClients()
                        .AddSingleton<IUserSettingsService, LocalUserSettingsService>(sp =>
                        {
                            var logger = sp.GetRequiredService<ILogger<LocalUserSettingsService>>();
                            var endpointOptions = sp.GetRequiredService<IOptions<EndpointsOptions>>();
                            return new LocalUserSettingsService(
                                logger,
                                endpointOptions,
                                Environment.SpecialFolder.ApplicationData);
                        })
                        ;
                });

            return await hostBuilder.RunCommandLineApplicationAsync<Program>(args);
        }

        /// <summary>
        /// This method is only executed if no subcommand can be matched to the provided args.
        /// </summary>
        protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return await Task.FromResult(1);
        }

        private static string GetVersion()
            => typeof(Program)
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion ?? "No version information available.";
    }
}
