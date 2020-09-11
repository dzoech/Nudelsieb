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
        private static string RelativeLocation => Path.Combine("nudelsieb", "user.settings");
        private readonly string _absoluteLocation;

        public LocalUserSettingsService(
            ILogger<LocalUserSettingsService> logger,
            Environment.SpecialFolder baseLocation = Environment.SpecialFolder.ApplicationData)
        {
            _logger = logger;
            _absoluteLocation = Path.Combine(Environment.GetFolderPath(baseLocation), RelativeLocation);
            Directory.CreateDirectory(Path.GetDirectoryName(_absoluteLocation));
        }

        /// <summary>
        /// Reads the local config file from disk into a <see cref="UserSettingsModel"/>.
        /// </summary>
        public async Task<UserSettingsModel> Read()
        {
            if (!File.Exists(_absoluteLocation))
            {
                await InitializeFile(_absoluteLocation);
            }

            using (var file = File.OpenRead(_absoluteLocation))
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
            using (var file = File.OpenWrite(_absoluteLocation))
            {
                await JsonSerializer.SerializeAsync(file, settings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.LogDebug($"Initializing file for {nameof(UserSettingsModel)} at {_absoluteLocation}");
            }
        }
    }
}
