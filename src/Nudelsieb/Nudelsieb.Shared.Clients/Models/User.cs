using System;
using System.Collections.Generic;
using System.Text;

namespace Nudelsieb.Shared.Clients.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string GivenName { get; set; }

        public string Email { get; set; }

        public User(string givenName, string email)
        {
            GivenName = givenName;
            Email = email;
        }
    }
}
