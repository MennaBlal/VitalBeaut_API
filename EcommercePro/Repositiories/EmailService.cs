
using EcommercePro.DTO;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Client;
using MimeKit;

namespace EcommercePro.Repositiories
{
    public class EmailService : IEmailService
    {
        private IConfiguration configuration ;
        private EmailSettings settings ;
        public EmailService(IConfiguration _configuration) {

            configuration = _configuration;
           settings = new EmailSettings();
            this.configuration.GetSection("emailSettings").Bind(settings);

        }
        
        public async Task<string> SendEmail( string Email ,string Meassage)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                     client.Connect(settings.host,settings.port );
                    client.Authenticate(settings.FromEmail, settings.password);
                    var bodybuilder = new BodyBuilder
                    {
                        HtmlBody = $"{Meassage}",
                        TextBody = "wellcome",
                    };
                    var message = new MimeMessage
                    {
                        Body = bodybuilder.ToMessageBody()
                    };
                    message.From.Add(new MailboxAddress("VitalBeauty website", settings.FromEmail));
                    message.To.Add(new MailboxAddress("testing", Email));
                     client.Send(message);
                     client.Disconnect(true);
                }
                //end of sending email
                return "Success";
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }
    }
}
