using GymMembershipAPI.DTO.Mail;
using GymMembershipAPI.Service;

namespace GymMembershipAPI.Mail
{
    public interface IEmailService
    {
        Task<ResponseModel<string>>SendEmailAsync(MailRequest mail);
    }
}
