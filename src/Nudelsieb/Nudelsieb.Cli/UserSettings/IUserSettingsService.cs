using System.Threading.Tasks;

namespace Nudelsieb.Cli.UserSettings
{
    public interface IUserSettingsService
    {
        public string Location { get; }
        Task<UserSettingsModel> Read();
        Task Write(UserSettingsModel settings);
    }
}