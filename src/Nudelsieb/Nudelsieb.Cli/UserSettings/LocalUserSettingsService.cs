using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nudelsieb.Cli.Options;
using static Nudelsieb.Cli.UserSettings.UserSettingsModel;

namespace Nudelsieb.Cli.UserSettings
{
    public class LocalUserSettingsService : IUserSettingsService
    {
        private readonly ILogger _logger;
        private readonly IOptions<EndpointsOptions> endpointOptions;

        public LocalUserSettingsService(
            ILogger<LocalUserSettingsService> logger,
            IOptions<EndpointsOptions> endpointOptions,
            Environment.SpecialFolder baseLocation)
        {
            _logger = logger;
            this.endpointOptions = endpointOptions;
            Location = Path.Combine(Environment.GetFolderPath(baseLocation), RelativeLocation);
            Directory.CreateDirectory(Path.GetDirectoryName(Location));
        }

        /// <summary>
        /// Returns the absolute path of the user settings file.
        /// </summary>
        public string Location { get; }

        private static string RelativeLocation => Path.Combine("nudelsieb", "settings.json");

        /// <summary>
        /// Reads the local config file from disk into a <see cref="UserSettingsModel"/>.
        /// </summary>
        public async Task<UserSettingsModel> ReadAsync()
        {
            if (!File.Exists(Location))
            {
                await InitializeFile(Location);
            }

            using (var file = File.OpenRead(Location))
            {
                return await JsonSerializer.DeserializeAsync<UserSettingsModel>(file);
            }
        }

        public async Task Write(UserSettingsModel settings)
        {
            using (var file = File.OpenWrite(Location))
            {
                var pos = file.Position;

                await JsonSerializer.SerializeAsync(file, settings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.LogDebug($"Initializing file for {nameof(UserSettingsModel)} at {Location}");
            }
        }

        public void SwitchEndpoint(EndpointSetting endpoint)
        {
            (endpoint.Value, endpoint.Previous) = (endpoint.Previous, endpoint.Value);
        }

        public void SetEndpoint(EndpointSetting endpoint, string value)
        {
            endpoint.Previous = endpoint.Value;
            endpoint.Value = new Uri(value);
        }

        private async Task InitializeFile(string location)
        {
            if (File.Exists(location))
            {
                throw new ArgumentException($"File {location} already exists.", nameof(location));
            }

            var defaultUserSettings = new UserSettingsModel();
            var applicationDefaultEndpoint = endpointOptions.Value.Braindump?.Value;

            if (applicationDefaultEndpoint != null)
            {
                defaultUserSettings.Endpoints.Braindump.Value = new Uri(applicationDefaultEndpoint);
            }

            await Write(defaultUserSettings);
        }
    }
}
