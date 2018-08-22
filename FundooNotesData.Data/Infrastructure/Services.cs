using FundooNotesData.Data.Domain;
using FundooNotesData.Data.Domain.Twilio;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FundooNotesData.Data.Infrastructure
{

    public class EmailService : IIdentityMessageService
    {

        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            //return Task.FromResult(0);    
            var mess = new MailMessage();
            mess.To.Add(new MailAddress(message.Destination));  // replace with valid value 
            mess.From = new MailAddress("bridgelabzsolutions@gmail.com");  // replace with valid value
            mess.Subject = message.Subject;
            mess.Body = string.Format(message.Body, message.Destination, mess.From);
            mess.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "bridgelabzsolutions@gmail.com",  // replace with valid value
                    Password = "bridgelabz"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com ";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mess);

            }
            return Task.FromResult(0);
        }
    }
    public class SmsService : IIdentityMessageService
    {
        private readonly ITwilioMessageSender _messageSender;

        public SmsService() : this(new TwilioMessageSender()) { }

        public SmsService(ITwilioMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task SendAsync(IdentityMessage message)
        {
            
            await _messageSender.SendMessageAsync(message.Destination,
                                                 Config.TwilioNumber,
                                                 message.Body);

        }
    }

}



