using System.Threading.Tasks;

namespace Nudelsieb.Cli.UserSettings
{
    public interface IUserSettingsService
    {
        public string Location { get; }
        Task<UserSettingsModel> ReadAsync();
        void SetEndpoint(UserSettingsModel.EndpointSetting endpoint, string value);
        void SwitchEndpoint(UserSettingsModel.EndpointSetting endpoint);
        Task Write(UserSettingsModel settings);
    }
}