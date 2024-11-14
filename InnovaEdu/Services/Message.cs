using ColegioWeb.Models.DTOs;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ColegioWeb.Services
{
    public class Message : IMessage
    {

        public GmailSettings _gmailsettings { get; }


        public Message(IOptions < GmailSettings> gmailSettings) { 
        _gmailsettings = gmailSettings.Value;
        }

        public void SendEmail(string asunto, string body, string to)
        {

            try {

                var fromEmail = _gmailsettings.Username;
                var password = _gmailsettings.Password;

                var message = new MailMessage();
                message.From = new MailAddress(fromEmail!);
                message.Subject = asunto;
                message.To.Add(new MailAddress(to));
                message.Body = body;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {

                    Port = _gmailsettings.Port,
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true    

                };

                smtpClient.Send(message);   

            }
            catch (Exception ex) 
            {
                throw new Exception("No se pudo enviar el email", ex);
            }



         
        }
    }
}
