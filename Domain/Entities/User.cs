using System;
using Domain.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<string>
    {
        protected User() { }

        public User(string fullName, string username, string email)
        {
            Id = Guid.NewGuid().ToString();
            FullName = fullName ?? "Anonymous";
            FirstName = fullName.ToFirstName();
            LastName = fullName.ToLastName();
            Email = email;
            UserName = username.ToUserName();
            NormalizedEmail = email.ToUpper();
            NormalizedUserName = email.ToUpper();
        }

        public string FullName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public override string ToString() => $"{FirstName} {LastName}";


        public void ChangeName(string fullName)
        {
            FullName = fullName;
            FirstName = fullName.ToFirstName();
            LastName = fullName.ToLastName();
        }
    }
}