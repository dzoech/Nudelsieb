using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Nudelsieb.Shared.Clients.Models;

namespace Nudelsieb.Shared.Clients.Authentication
{
    public interface IAuthenticationService
    {
        Task<(bool Success, JwtSecurityToken? AccessToken)> GetCachedAccessTokenAsync();
        Task<(JwtSecurityToken IdToken, JwtSecurityToken AccessToken)> LoginAsync();
        User GetUserFromIdToken(JwtSecurityToken idToken);
    }
}