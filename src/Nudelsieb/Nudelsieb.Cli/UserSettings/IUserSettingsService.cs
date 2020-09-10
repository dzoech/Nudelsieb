using System.Threading.Tasks;

namespace Nudelsieb.Cli.UserSettings
{
    public interface IUserSettingsService
    {
        Task<UserSettingsModel> Read();
        Task Write(UserSettingsModel settings);
    }
}