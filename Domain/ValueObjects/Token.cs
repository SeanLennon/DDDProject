using System;

namespace Domain.ValueObjects
{
    public class Token
    {
        public Token(string token) => AccessToken = token;

        public string AccessToken { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime Expires { get; private set; } = DateTime.Now.AddHours(12);
    }
}