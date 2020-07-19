using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Nudelsieb.Cli.Models;

namespace Nudelsieb.Cli.Services
{
    interface IAuthenticationService
    {
        Task<(bool Success, JwtSecurityToken? AccessToken)> GetCachedAccessTokenAsync();
        Task<(JwtSecurityToken IdToken, JwtSecurityToken AccessToken)> LoginAsync();
        User GetUserFromIdToken(JwtSecurityToken idToken);
    }
}