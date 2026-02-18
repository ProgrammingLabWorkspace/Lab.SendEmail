
using Lab.SendEmail.Core.Contracts;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Lab.SendEmail.Infra.Services
{
    public class EmailServiceConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool SandboxMode { get; private set; }
        public string SandboxTOEmail { get; set; }

        public void ActiveSandboxMode()
        {
            if (string.IsNullOrEmpty(SandboxTOEmail)) throw new Exception("É necessário informar um email destinatário para as mensagens para ativar o modo Sandbox.");

            SandboxMode = true;
        }
    }

    public class EmailService(EmailServiceConfiguration configuration) : IEmailService
    {
        private EmailServiceConfiguration Configuration { get; set; } = configuration;

        public async Task<bool> SendEmail(List<string> to, string subject, string message)
        {
            var smtpHost = Configuration.Host;
            var smtpPort = Configuration.Port;
            var smtpUser = Configuration.User;
            var smtpPassword = Configuration.Password;

            var toEmails = new List<string>(to);

            if (Configuration.SandboxMode)
            {
                toEmails.Clear();
                toEmails.Add(Configuration.SandboxTOEmail);
            }

            var mMessage = new MimeMessage();
            mMessage.From.Add(new MailboxAddress("Lab.SendEmail", smtpUser));
            mMessage.To.AddRange(toEmails.Select(t => new MailboxAddress("Destinatário", t)));
            mMessage.Subject = subject;

            mMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            using var client = new SmtpClient();

            try
            {
                // Connect to the SMTP server
                // Use SecureSocketOptions.StartTls for secure connections
                await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);

                // Authenticate with credentials (or OAuth2 if supported)
                await client.AuthenticateAsync(smtpUser, smtpPassword);

                // Send the message
                await client.SendAsync(mMessage);
                Console.WriteLine("Email sent successfully!");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
            finally
            {
                // Disconnect from the server
                await client.DisconnectAsync(true);
            }
        }
    }
}
