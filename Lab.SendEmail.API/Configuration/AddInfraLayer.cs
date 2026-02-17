using Lab.SendEmail.Core.Contracts;
using Lab.SendEmail.Infra.Services;

namespace Lab.SendEmail.API.Configuration
{
    public static class AddInfraLayer
    {
        public static WebApplicationBuilder AddInfra(this WebApplicationBuilder builder)
        {
            AddEmailService(builder);

            return builder;
        }

        private static WebApplicationBuilder AddEmailService(WebApplicationBuilder builder)
        {
            var config = builder
                .Configuration
                .GetSection("EmailServiceConfiguration")
                .Get<EmailServiceConfiguration>();

            if (config is null)
            {
                throw new Exception("Atenção! É necessário definir as configurações de email no arquivo appsettings.json ou via User Secrets.");
            }

            if (Util.IsDesenv())
            {
                config.ActiveSandboxMode("oliveirachristian1908@gmail.com");
            }

            builder
                .Services
                .AddScoped<IEmailService, EmailService>(opts =>
            {
                return new EmailService(config);
            });

            return builder;
        }
    }
}
