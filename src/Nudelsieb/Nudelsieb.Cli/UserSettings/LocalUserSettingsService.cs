using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nudelsieb.Cli.Options;

namespace Nudelsieb.Cli.UserSettings
{
    public class LocalUserSettingsService : IUserSettingsService
    {
        private readonly ILogger _logger;
        private readonly IOptions<EndpointsOptions> endpointOptions;

        private static string RelativeLocation => Path.Combine("nudelsieb", "settings.json");
        /// <summary>
        /// Returns the absolute path of the user settings file.
        /// </summary>
        public string Location { get; }

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
        /// Reads the local config file from disk into a <see cref="UserSettingsModel"/>.
        /// </summary>
        public async Task<UserSettingsModel> Read()
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

        private async Task InitializeFile(string location)
        {
            if (File.Exists(location))
            {
                throw new ArgumentException($"File {location} already exists.", nameof(location));
            }

            // TODO user options
            var defaultUserSettings = new UserSettingsModel();
            var appDefaultEndpoint = endpointOptions.Value.Braindump?.Value;

            if (appDefaultEndpoint != null)
            {
                defaultUserSettings.Endpoints.Braindump.Set(appDefaultEndpoint);
            }

            await Write(defaultUserSettings);
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
    }
}
