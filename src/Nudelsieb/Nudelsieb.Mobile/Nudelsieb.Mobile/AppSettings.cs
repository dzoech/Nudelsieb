using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Nudelsieb.Mobile.Configuration;
using Nudelsieb.Shared.Clients.Authentication;

namespace Nudelsieb.Mobile
{
    public class AppSettings
    {
        private const string AppSettingsFileName = "appsettings.json";
        private const string SecretFileName = "secrets.json";

        private static AppSettings instance;

        public static AppSettings Settings => instance ??= Initialize();
        public AuthOptions Auth { get; set; } = new AuthOptions();
        public EndpointsOptions Endpoints { get; set; } = new EndpointsOptions();
        public NotificationsOptions Notifications { get; set; } = new NotificationsOptions();

        /// <summary>
        /// Tag used in log messages to easily filter the device log during development.
        /// </summary>
        public string DebugTag { get; set; }

        /// <summary>
        /// Set to a unique value for your app, such as your bundle identifier. Used on iOS to share
        /// keychain access.
        /// </summary>
        public string IosKeychainSecurityGroups { get; set; }

        public static AppSettings Initialize()
        {
            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            string appSettingsContent = ReadSettingsFile(AppSettingsFileName);
            var appSettings = JsonSerializer.Deserialize<AppSettings>(appSettingsContent, options);

            try
            {
                string secretContent = ReadSettingsFile(SecretFileName);
                var secretSettings = JsonSerializer.Deserialize<AppSettings>(secretContent, options);
                OverrideSettings(source: secretSettings, target: appSettings);
            }
            catch (IOException ex)
            {
                Console.WriteLine(
                    $"Error, could not apply {nameof(AppSettings)} from {SecretFileName} " +
                    $"({ex.Message})");
            }

            return appSettings;
        }

        private static string ReadSettingsFile(string fileName)
        {
            var assembly = Assembly.GetAssembly(typeof(AppSettings));
            var resourceName = $"{assembly.GetName().Name}.{fileName}";
            var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream is null)
            {
                throw new FileNotFoundException(
                    $"Could not find manifest resource '{resourceName}'", resourceName);
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Overrides a <typeparamref name="TSettings"/> object with all non-null values of another
        /// <typeparamref name="TSettings"/> object.
        /// </summary>
        /// <typeparam name="TSettings">A plain object</typeparam>
        /// <remarks>Overrides collections only if they are empty.</remarks>
        private static void OverrideSettings<TSettings>(in TSettings source, TSettings target)
            where TSettings : class
        {
            foreach (var prop in source.GetType().GetProperties())
            {
                if (!prop.CanWrite)
                    continue;

                if (prop.Name == nameof(Settings))
                    continue;

                var sourceValue = prop.GetValue(source);

                if (sourceValue is null)
                    continue;

                var isCollection =
                    typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                    prop.PropertyType != typeof(string);

                var isNativeType = prop.PropertyType.Namespace.StartsWith("System");

                if (isCollection)
                {
                    var targetCollection = prop.GetValue(target) as IEnumerable;
                    var isEmpty = targetCollection.GetEnumerator().MoveNext() == false;

                    if (isEmpty)
                    {
                        prop.SetValue(target, sourceValue);
                    }
                    else
                    {
                        Debug.WriteLine(
                            "Cannot override settings collection because" +
                            "it already contains items.");
                    }
                }
                else if (isNativeType)
                {
                    prop.SetValue(target, sourceValue);
                }
                else
                {
                    OverrideSettings(sourceValue, prop.GetValue(target));
                }
            }
        }
    }
}
