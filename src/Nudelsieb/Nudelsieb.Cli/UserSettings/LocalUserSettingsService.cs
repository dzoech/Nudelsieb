using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Nudelsieb.Cli.UserSettings
{
    public class LocalUserSettingsService : IUserSettingsService
    {
        private readonly ILogger _logger;
        private static string RelativeLocation => Path.Combine("nudelsieb", "settings.json");
        /// <summary>
        /// Returns the absolute path of the user settings file.
        /// </summary>
        public string Location { get; }

        public LocalUserSettingsService(    
            ILogger<LocalUserSettingsService> logger,
            Environment.SpecialFolder baseLocation)
        {
            _logger = logger;
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

            await Write(new UserSettingsModel());
        }

        public async Task Write(UserSettingsModel settings)
        {
            using (var file = File.OpenWrite(Location))
            {
                await JsonSerializer.SerializeAsync(file, settings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.LogDebug($"Initializing file for {nameof(UserSettingsModel)} at {Location}");
            }
        }
    }
}
