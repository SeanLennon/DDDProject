using System;

namespace DDDProjectConsole
{
    public class EmailMessage
    {
        public string Email { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }

        public override string ToString() => $"{Email} {Body} {Subject}";

        public static explicit operator EmailMessage(string model)
        {
            var message = model.Split(',');
            return new EmailMessage()
            {
                Email = message[0].Trim(),
                Body = message[1].Trim(),
                Subject = message[2].Trim()
            };
        }
    }
}