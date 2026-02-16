
namespace Lab.SendEmail.Infra.Services
{
    public class EmailService(EmailServiceConfiguration configuration) : BaseEmailService(configuration)
    {
        public override Task<bool> SendEmail(string to, string subject, string message)
        {
            return base.SendEmail(to, subject, message);
        }
    }
}
