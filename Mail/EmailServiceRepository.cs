using GymMembershipAPI.DTO.Mail;
using GymMembershipAPI.Service;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GymMembershipAPI.Mail
{
    public class EmailServiceRepository(IOptions<Setup> setup) : IEmailService
    {
        private readonly Setup _setup = setup.Value;

        public async Task<ResponseModel<string>> SendEmailAsync(MailRequest mail)
        {
            try
            {
             var response = new ResponseModel<string>();
                //Using MimeKit 
                var message = new MimeMessage
                {
                    To = { MailboxAddress.Parse(mail.Reciever) },
                    Sender = MailboxAddress.Parse(_setup.Sender),
                    Subject = mail.Subject,
                    Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = mail.Content
                    }
                };
                //Inject SmtpClient
                  using (var client = new SmtpClient())
                {
                    client.Connect(_setup.Server, _setup.Port);
                    client.Authenticate(_setup.Sender, _setup.Password);
                   await client.SendAsync(message);
                    client.Disconnect(true);
                }
                return response = response.SuccessResult("Mail send successfully");
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
