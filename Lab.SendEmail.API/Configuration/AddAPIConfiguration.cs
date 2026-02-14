namespace Lab.SendEmail.API.Configuration
{
    public static class AddAPIConfiguration
    {
        public static WebApplicationBuilder AddAPI (this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            return builder;
        }
    }
}
