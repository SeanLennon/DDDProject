using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Domain.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        protected User() { }

        public User(string fullName, string username, string email, IList<string> roles)
        {
            FullName = fullName ?? "Anonymous";
            FirstName = FullName.ToFirstName();
            LastName = FullName.ToLastName();
            UserName = username.ToUserName();
            Email = email;
            NormalizedEmail = email.ToUpper();
            NormalizedUserName = UserName.ToUpper();
            Roles = roles;
            Claims = new List<Claim>
            {
                new Claim("user_name", UserName),
                new Claim("full_name", FullName),
                new Claim("email", Email),
                new Claim("user_id", Id),
            };
        }

        public string FullName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        [NotMapped]
        public IEnumerable<Claim> Claims { get; private set; }
        [NotMapped]
        public IList<string> Roles { get; private set; }


        public override string ToString() => $"{FirstName} {LastName}";

        public void AddClaims(IEnumerable<Claim> claims) => Claims = claims;
        // public void AddRoles(IList<string> roles) => Roles = roles;
        public void ChangeName(string fullName)
        {
            FullName = fullName;
            FirstName = fullName.ToFirstName();
            LastName = fullName.ToLastName();
        }
    }
}