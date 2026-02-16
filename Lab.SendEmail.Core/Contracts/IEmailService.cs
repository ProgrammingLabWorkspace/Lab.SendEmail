namespace Lab.SendEmail.Core.Contracts
{
    public interface IEmailService
    {
        public Task<bool> SendEmail(string to, string subject, string message);
    }
}
