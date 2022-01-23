using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Nudelsieb.WebApi
{
    public class UserService
    {
        private const string SubjectClaim = "sub";
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Guid GetActiveUserId()
        {
            string sub = httpContextAccessor.HttpContext.User.FindFirstValue(SubjectClaim);
            if (sub == null)
                throw new Exception($"Could not access user claim '{SubjectClaim}'");

            return Guid.Parse(sub);
        }
    }
}
