namespace Lab.SendEmail.Core.Contracts
{
    public interface IEmailService
    {
        public Task<bool> SendEmail(List<string> to, string subject, string message);
    }
}
