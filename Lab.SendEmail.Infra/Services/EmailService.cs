
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
        public bool SandboxMode { get; set; }
        public string SandboxTOEmail { get; set; }

        public void ActiveSandboxMode(string sandboxTOEmail)
        {
            SandboxMode = true; 
            SandboxTOEmail = sandboxTOEmail;
        }
    }

    public class EmailService(EmailServiceConfiguration configuration) : IEmailService
    {
        private EmailServiceConfiguration Configuration { get; set; } = configuration;

        public async Task<bool> SendEmail(string to, string subject, string message)
        {
            var smtpHost = Configuration.Host;
            var smtpPort = Configuration.Port;
            var smtpUser = Configuration.User;
            var smtpPassword = Configuration.Password;

            if (Configuration.SandboxMode)
            {
                if (string.IsNullOrEmpty(Configuration.SandboxTOEmail)) throw new ArgumentNullException("When sandbox mode is true, you must inform SandboxTOEmail.");

                to = Configuration.SandboxTOEmail;
            }

            var mMessage = new MimeMessage();
            mMessage.From.Add(new MailboxAddress("Sender Name", smtpUser));
            mMessage.To.Add(new MailboxAddress("Recipient Name", to));
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
