using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DDDProjectConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "email", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        EmailMessage message = (EmailMessage)Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine("Received {0}", message.ToString());

                        using var smtp = new SmtpClient("smtp.outlook.com")
                        {
                            Port = 587,
                            Credentials = new NetworkCredential("sean.dev@outlook.com.br", "Affmano@25240000"),
                            EnableSsl = true
                        };

                        using var mail = new MailMessage()
                        {
                            From = new MailAddress(message.Email),
                            Subject = message.Subject,
                            Body = message.Body,
                            IsBodyHtml = true,
                        };
                        mail.To.Add(message.Email);
                        try
                        {
                            smtp.SendMailAsync(mail);
                        }
                        catch (SmtpException ex)
                        {
                            throw new SmtpException(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    };
                    channel.BasicConsume(queue: "email",
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
