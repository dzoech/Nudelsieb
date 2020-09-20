using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using Nudelsieb.Cli.Options;
using Nudelsieb.Cli.RestClients;
using Nudelsieb.Cli.Services;
using Nudelsieb.Cli.UserSettings;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Nudelsieb.Cli
{
    // See example: https://github.com/natemcmaster/CommandLineUtils/blob/master/docs/samples/subcommands/inheritance/Program.cs
    [HelpOption("-?|-h|--help")]
    abstract class CommandBase
    {
        protected virtual async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            //Console.WriteLine("Result = nudelsieb " + ArgumentEscaper.EscapeAndConcatenate(args));
            return await Task.FromResult(0);
        }
    }

    [Command("nudelsieb")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(GetCommand),
        typeof(AddCommand),
        typeof(LoginCommand),
        typeof(ConfigCommand))]
    class Program : CommandBase
    {
        private const Environment.SpecialFolder UserSettingsLocation = Environment.SpecialFolder.ApplicationData;

        public static async Task<int> Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(configBuilder =>
                {
                    configBuilder.AddJsonFile("hostsettings.json", optional: true);
                })
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    // platform-independent way to get assembly path
                    // see https://github.com/dotnet/runtime/issues/13051#issuecomment-535654457
                    var executingAssembly = Process.GetCurrentProcess().MainModule.FileName;

                    configBuilder.SetBasePath(Path.GetDirectoryName(executingAssembly));
                    configBuilder.AddJsonFile("appsettings.json");

                    var location = Environment.GetFolderPath(UserSettingsLocation);
                    // TODO sub dir and file name are defined here and in LocalUserSettingsService
                    // inject sub dir and file name settings into LocalUserSettingsService 
                    var settingsFile = Path.Combine(location, "nudelsieb", "settings.json");
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
                    var authOptions = new AuthOptions();
                    context.Configuration.GetSection(AuthOptions.SectionName).Bind(authOptions);

                    services.Configure<AuthOptions>(o => o = authOptions);

                    services.Configure<EndpointsOptions>(o =>
                        context.Configuration.GetSection(
                            EndpointsOptions.SectionName).Bind(o));

                    // using Microsoft.Identity.Client.Extensions.Msal as Cache
                    // https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/tree/master/src/Microsoft.Identity.Client.Extensions.Msal
                    var props = new StorageCreationPropertiesBuilder(
                            authOptions.Cache.FileName,
                            authOptions.Cache.Directory,
                            authOptions.ClientId)
                        .Build();

                    // composition root
                    services
                        .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                        .AddSingleton<IPublicClientApplication>(_ =>
                        {
                            var app = PublicClientApplicationBuilder
                              .Create(authOptions.ClientId)
                              .WithRedirectUri(authOptions.RedirectUri) // AAD B2C requires a port https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/System-Browser-on-.Net-Core#limitations
                              .WithB2CAuthority(authOptions.B2cAuthority)
                              .Build();

                            var msalCacheHelper = MsalCacheHelper.CreateAsync(props).Result;
                            msalCacheHelper.RegisterCache(app.UserTokenCache);

                            return app;
                        })
                        .AddTransient<IBraindumpService, BraindumService>()
                        .AddTransient<IAuthenticationService, AuthenticationService>()
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
