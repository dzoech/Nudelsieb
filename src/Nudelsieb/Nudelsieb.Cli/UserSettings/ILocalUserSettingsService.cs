using System.Threading.Tasks;

namespace Nudelsieb.Cli.UserSettings
{
    public interface ILocalUserSettingsService
    {
        string AbsoluteLocation { get; }

        Task<UserSettingsModel> Read();
        Task Write(UserSettingsModel settings);
    }
}